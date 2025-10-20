using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using yMoi.Dto;
using yMoi.Dto.Medicine;
using yMoi.Dto.Service;
using yMoi.Model;
using yMoi.Service.Interfaces;

namespace yMoi.Service
{
    public class MedicineService : IMedicineService
    {
        private readonly ApplicationDbContext _dbContext;

        public MedicineService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResponseModel> CreateMedicine(CreateMedicineDto dto, int createById)
        {
            var existCode = await _dbContext.Medicines.Where(a => a.Code == dto.Code && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null)
            {
                return JsonResponse.Error(0, "Mã thuốc đã tồn tại");
            }

            var medicine = new Model.Medicine
            {
                Code = dto.Code,
                Name = dto.Name,
                Status = dto.Status,
                UnitId = dto.UnitId,
                ImportPrice = dto.ImportPrice,
                ImportPriceUnitId = dto.ImportPriceUnitId,
                OfficialPrice = dto.OfficialPrice,
                OfficialPriceUnitId = dto.OfficialPriceUnitId,
                Quantity = dto.Quantity,
                Manufacturer = dto.Manufacturer,
                Supplier = dto.Supplier,
                Note = dto.Note,
                PackingSpecification = dto.PackingSpecification,
                CreatedById = createById,
            };

            await _dbContext.Medicines.AddAsync(medicine);
            await _dbContext.SaveChangesAsync();

            foreach (var file in dto.Files)
            {
                var medicineFile = new MedicineFile
                {
                    Name = file.Name,
                    Url = file.Url,
                    MedicineId = medicine.Id
                };
                await _dbContext.MedicineFiles.AddAsync(medicineFile);
            }

            await _dbContext.SaveChangesAsync();

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> DeleteMedicine(int id)
        {
            var medicine = await _dbContext.Medicines.Where(a => a.Id == id && a.IsActive == true).FirstOrDefaultAsync();
            if (medicine != null)
            {
                medicine.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> EditMedicine(int id, CreateMedicineDto dto)
        {
            var existCode = await _dbContext.Medicines.Where(a => a.Id != id && a.Code == dto.Code && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null)
            {
                return JsonResponse.Error(0, "Mã thuốc đã tồn tại");
            }

            var medicine = await _dbContext.Medicines.Where(a => a.Id == id && a.IsActive == true).FirstOrDefaultAsync();

            if (medicine == null)
            {
                return JsonResponse.Error(0, "Thuốc không tồn tại");
            }

            medicine.Code = dto.Code;
            medicine.Name = dto.Name;
            medicine.Status = dto.Status;
            medicine.UnitId = dto.UnitId;

            medicine.ImportPrice = dto.ImportPrice;
            medicine.ImportPriceUnitId = dto.ImportPriceUnitId;
            medicine.OfficialPrice = dto.OfficialPrice;
            medicine.OfficialPriceUnitId = dto.OfficialPriceUnitId;

            medicine.Quantity = dto.Quantity;
            medicine.Manufacturer = dto.Manufacturer;
            medicine.Supplier = dto.Supplier;
            medicine.Note = dto.Note;
            medicine.PackingSpecification = dto.PackingSpecification;

            var existFileIds = dto.Files.Where(a => a.Id.HasValue).Select(a => a.Id.Value).ToList();

            var deleteMedicines = await _dbContext.MedicineFiles.Where(a => !existFileIds.Contains(a.Id) && a.IsActive == true && a.MedicineId == medicine.Id).ToListAsync();

            foreach (var deleteMedicine in deleteMedicines)
            {
                deleteMedicine.IsActive = false;
            }

            await _dbContext.SaveChangesAsync();

            foreach (var file in dto.Files)
            {
                var medicineFile = await _dbContext.MedicineFiles.Where(a => a.Id == file.Id).FirstOrDefaultAsync();

                if (medicineFile == null)
                {
                    medicineFile = new MedicineFile
                    {
                        Name = file.Name,
                        Url = file.Url,
                        MedicineId = medicine.Id
                    };
                    await _dbContext.MedicineFiles.AddAsync(medicineFile);
                }
                else
                {
                    medicineFile.Name = file.Name;
                    medicineFile.Url = file.Url;
                }
            }

            await _dbContext.SaveChangesAsync();
            return JsonResponse.Success(medicine);
        }

        public async Task<JsonResponseModel> GetListMedicine(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.Medicines.Where(a => a.IsActive == true
                            && (status.HasValue ? a.Status == status : true)
                            && (!string.IsNullOrEmpty(search) ? a.Code.Contains(search) || a.Name.Contains(search) : true)
                            && (fromDate.HasValue ? a.CreatedDate >= fromDate : true)
                            && (toDate.HasValue ? a.CreatedDate <= toDate : true)
                        );
            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListMedicine
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                CreatedDate = a.CreatedDate,
                Status = a.Status,
                UnitId = a.UnitId,
                UnitName = a.Unit.Name,
                Quantity = a.Quantity
            }).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return JsonResponse.Success(list, new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = count
            });
        }

        public async Task<JsonResponseModel> GetMedicineDetails(int id)
        {
            var details = await _dbContext.Medicines.Where(a => a.Id == id).Select(a => new GetMedicineDetailsModel
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                Status = a.Status,
                UnitId = a.UnitId,
                ImportPrice = a.ImportPrice,
                ImportPriceUnitId = a.ImportPriceUnitId,
                OfficialPrice = a.OfficialPrice,
                OfficialPriceUnitId = a.OfficialPriceUnitId,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                CreatedDate = a.CreatedDate,
                Quantity = a.Quantity,
                Manufacturer = a.Manufacturer,
                Supplier = a.Supplier,
                Note = a.Note,
                PackingSpecification = a.PackingSpecification,
                Files = a.MedicineFiles.Where(b => b.IsActive == true).Select(b => new CreateMedicineFileDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Url = b.Url
                }).ToList(),
            }).FirstOrDefaultAsync();

            return JsonResponse.Success(details);
        }

        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            var medicine = await _dbContext.Medicines.Where(a => a.Id == id && a.IsActive == true).FirstOrDefaultAsync();
            if (medicine != null)
            {
                medicine.Status = !medicine.Status;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }
    }
}
