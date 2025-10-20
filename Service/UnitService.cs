using Microsoft.EntityFrameworkCore;
using yMoi.Dto;
using yMoi.Dto.Unit;
using yMoi.Service.Interfaces;

namespace yMoi.Service
{
    public class UnitService : IUnitService
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResponseModel> CreateUnit(CreateUnitDto dto, int createById)
        {
            var existCode = await _dbContext.Units.Where(a => a.Code == dto.Code && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null) return JsonResponse.Error(0, "Mã phòng ban đã tồn tại");

            var Unit = new Model.Unit
            {
                Code = dto.Code,
                Name = dto.Name,
                Status = dto.Status,
                CreatedById = createById,
                Note = dto.Note
            };

            await _dbContext.Units.AddAsync(Unit);
            await _dbContext.SaveChangesAsync();

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> DeleteUnit(int id)
        {
            var Unit = await _dbContext.Units.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (Unit != null)
            {
                Unit.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> EditUnit(int id, CreateUnitDto dto)
        {
            var existCode = await _dbContext.Units.Where(a => a.Code == dto.Code && a.Id != id && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null) return JsonResponse.Error(0, "Mã phòng ban đã tồn tại");

            var deparment = await _dbContext.Units.Where(a => a.IsActive == true && a.Id == id).FirstOrDefaultAsync();

            if (deparment == null) return JsonResponse.Error(0, "Phòng ban không tồn tại");

            deparment.Code = dto.Code;
            deparment.Name = dto.Name;
            deparment.Status = dto.Status;
            deparment.Note = dto.Note;

            await _dbContext.SaveChangesAsync();

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> GetUnitDetails(int id)
        {
            var model = await _dbContext.Units.Where(a => a.Id == id).OrderByDescending(a => a.Id).Select(a => new GetListUnitDto
            {
                Id = a.Id,
                Status = a.Status,
                CreatedDate = a.CreatedDate,
                Name = a.Name ?? string.Empty,
                Code = a.Code,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name,
                Note = a.Note ?? string.Empty
            }).FirstOrDefaultAsync();

            return JsonResponse.Success(model);
        }


        public async Task<JsonResponseModel> GetListUnit(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.Units.Where(a => a.IsActive == true
                && (status.HasValue ? a.Status == status : true)
                && (!string.IsNullOrEmpty(search) ? a.Code.Contains(search) || a.Name.Contains(search) : true)
                && (fromDate.HasValue ? a.CreatedDate >= fromDate : true)
                && (toDate.HasValue ? a.CreatedDate <= toDate : true)
            );
            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListUnitDto
            {
                Id = a.Id,
                Status = a.Status,
                CreatedDate = a.CreatedDate,
                Name = a.Name,
                Code = a.Code,
                CreatedById = a.CreatedById,
                CreatedByName = a.CreatedBy.Name
            }).Skip((page - 1) * limit).Take(limit).ToListAsync();

            return JsonResponse.Success(list, new PagingModel
            {
                Page = page,
                Limit = limit,
                TotalItemCount = count
            });
        }

        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            var found = await _dbContext.Units.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (found != null)
            {
                found.Status = !found.Status;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }
    }
}