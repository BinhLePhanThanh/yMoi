using Microsoft.EntityFrameworkCore;
using System.Transactions;
using yMoi.Dto;
using yMoi.Dto.Service;
using yMoi.Dto.Unit;
using yMoi.Model;
using yMoi.Service.Interfaces;

namespace yMoi.Service
{
    public class ServiceService : IServiceService
    {
        private readonly ApplicationDbContext _dbContext;

        public ServiceService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResponseModel> CreateService(CreateServiceDto dto, int createById)
        {

            var existCode = await _dbContext.Services.Where(a => a.Code == dto.Code && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null)
            {
                return JsonResponse.Error(0, "Mã dịch vụ đã tồn tại");
            }

            var service = new Model.Service
            {
                Code = dto.Code,
                Name = dto.Name,
                Status = dto.Status,
                UnitId = dto.UnitId,
                DepartmentId = dto.DepartmentId,
                Type = dto.Type,
                ImportPrice = dto.ImportPrice,
                ImportPriceUnitId = dto.ImportPriceUnitId,
                OfficialPrice = dto.OfficialPrice,
                OfficialPriceUnitId = dto.OfficialPriceUnitId,
                ReferenceLimit1 = dto.ReferenceLimit1,
                ReferenceLimit2 = dto.ReferenceLimit2,
                TurnaroundTime = dto.TurnaroundTime,
                ToolFeatures = dto.ToolFeatures,
                CreatedById = createById,
                Note = dto.Note
            };

            await _dbContext.Services.AddAsync(service);
            await _dbContext.SaveChangesAsync();

            foreach (var file in dto.Files)
            {
                var serviceFile = new ServiceFile
                {
                    Name = file.Name,
                    Url = file.Url,
                    ServiceId = service.Id
                };
                await _dbContext.ServiceFiles.AddAsync(serviceFile);
            }

            await _dbContext.SaveChangesAsync();

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> DeleteService(int id)
        {
            var service = await _dbContext.Services.Where(a => a.Id == id && a.IsActive == true).FirstOrDefaultAsync();
            if (service != null)
            {
                service.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> EditService(int id, CreateServiceDto dto)
        {
            var existCode = await _dbContext.Services.Where(a => a.Id != id && a.Code == dto.Code && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null)
            {
                return JsonResponse.Error(0, "Mã dịch vụ đã tồn tại");
            }

            var service = await _dbContext.Services.Where(a => a.Id == id && a.IsActive == true).FirstOrDefaultAsync();

            if (service == null)
            {
                return JsonResponse.Error(0, "Dịch vụ không tồn tại");
            }

            service.Code = dto.Code;
            service.Name = dto.Name;
            service.Status = dto.Status;
            service.UnitId = dto.UnitId;
            service.DepartmentId = dto.DepartmentId;
            service.Type = dto.Type;
            service.ImportPrice = dto.ImportPrice;
            service.ImportPriceUnitId = dto.ImportPriceUnitId;
            service.OfficialPrice = dto.OfficialPrice;
            service.OfficialPriceUnitId = dto.OfficialPriceUnitId;
            service.ReferenceLimit1 = dto.ReferenceLimit1;
            service.ReferenceLimit2 = dto.ReferenceLimit2;
            service.TurnaroundTime = dto.TurnaroundTime;
            service.ToolFeatures = dto.ToolFeatures;
            service.Note = dto.Note;

            var existFileIds = dto.Files.Where(a => a.Id.HasValue).Select(a => a.Id.Value).ToList();

            var deleteServices = await _dbContext.ServiceFiles.Where(a => !existFileIds.Contains(a.Id) && a.IsActive == true && a.ServiceId == service.Id).ToListAsync();

            foreach (var deleteService in deleteServices)
            {
                deleteService.IsActive = false;
            }

            await _dbContext.SaveChangesAsync();

            foreach (var file in dto.Files)
            {
                var serviceFile = await _dbContext.ServiceFiles.Where(a => a.Id == file.Id).FirstOrDefaultAsync();

                if (serviceFile == null)
                {
                    serviceFile = new ServiceFile
                    {
                        Name = file.Name,
                        Url = file.Url,
                        ServiceId = service.Id
                    };
                    await _dbContext.ServiceFiles.AddAsync(serviceFile);
                }
                else
                {
                    serviceFile.Name = file.Name;
                    serviceFile.Url = file.Url;
                }
            }

            await _dbContext.SaveChangesAsync();
            return JsonResponse.Success(service);
        }

        public async Task<JsonResponseModel> GetListService(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.Services.Where(a => a.IsActive == true
                            && (status.HasValue ? a.Status == status : true)
                            && (!string.IsNullOrEmpty(search) ? a.Code.Contains(search) || a.Name.Contains(search) : true)
                            && (fromDate.HasValue ? a.CreatedDate >= fromDate : true)
                            && (toDate.HasValue ? a.CreatedDate <= toDate : true)
                        );
            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListService
            {
                Id = a.Id,
                Name = a.Name,
                Code = a.Code,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                CreatedDate = a.CreatedDate,
                Status = a.Status,
                DepartmentId = a.DepartmentId,
                DepartmentName = a.Department.Name,
                Type = a.Type,
            }).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return JsonResponse.Success(list, new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = count
            });
        }

        public async Task<JsonResponseModel> GetServiceDetails(int id)
        {
            var details = await _dbContext.Services.Where(a => a.Id == id).Select(a => new GetServiceDetailsModel
            {
                Id = a.Id,
                Code = a.Code,
                Name = a.Name,
                Status = a.Status,
                UnitId = a.UnitId,
                DepartmentId = a.DepartmentId,
                Type = a.Type,
                ImportPrice = a.ImportPrice,
                ImportPriceUnitId = a.ImportPriceUnitId,
                OfficialPrice = a.OfficialPrice,
                OfficialPriceUnitId = a.OfficialPriceUnitId,
                ReferenceLimit1 = a.ReferenceLimit1,
                ReferenceLimit2 = a.ReferenceLimit2,
                TurnaroundTime = a.TurnaroundTime,
                ToolFeatures = a.ToolFeatures,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                CreatedDate = a.CreatedDate,
                Note = a.Note,
                Files = a.ServiceFiles.Where(b => b.IsActive == true).Select(b => new CreateServiceFileDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Url = b.Url,
                    CreatedDate = b.CreatedDate
                }).ToList(),
            }).FirstOrDefaultAsync();

            return JsonResponse.Success(details);
        }

        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            var service = await _dbContext.Services.Where(a => a.Id == id && a.IsActive == true).FirstOrDefaultAsync();
            if (service != null)
            {
                service.Status = !service.Status;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }
    }
}