using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using yMoi.Dto;
using yMoi.Dto.Department;
using yMoi.Model;
using yMoi.Service.Interfaces;

namespace yMoi.Service
{
    public class DepartmentService : IDepartmentService
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<JsonResponseModel> CreateDepartment(CreateDepartmentDto dto, int createById)
        {
            var existCode = await _dbContext.Departments.Where(a => a.Code == dto.Code && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null) return JsonResponse.Error(0, "Mã phòng ban đã tồn tại");

            var department = new Department
            {
                Code = dto.Code,
                Name = dto.Name,
                Status = dto.Status,
                CreatedById = createById,
                Note = dto.Note
            };

            await _dbContext.Departments.AddAsync(department);
            await _dbContext.SaveChangesAsync();

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> DeleteDepartment(int id)
        {
            var department = await _dbContext.Departments.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (department != null)
            {
                department.IsActive = false;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> EditDepartment(int id, CreateDepartmentDto dto)
        {
            var existCode = await _dbContext.Departments.Where(a => a.Code == dto.Code && a.Id != id && a.IsActive == true).FirstOrDefaultAsync();

            if (existCode != null) return JsonResponse.Error(0, "Mã phòng ban đã tồn tại");

            var deparment = await _dbContext.Departments.Where(a => a.IsActive == true && a.Id == id).FirstOrDefaultAsync();

            if (deparment == null) return JsonResponse.Error(0, "Phòng ban không tồn tại");

            deparment.Code = dto.Code;
            deparment.Name = dto.Name;
            deparment.Status = dto.Status;
            deparment.Note = dto.Note;

            await _dbContext.SaveChangesAsync();

            return JsonResponse.Success(new { });
        }

        public async Task<JsonResponseModel> GetDepartmentDetails(int id)
        {
            var model = await _dbContext.Departments.Where(a => a.Id == id).OrderByDescending(a => a.Id).Select(a => new GetListDepartmentDto
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


        public async Task<JsonResponseModel> GetListDepartment(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate)
        {
            var query = _dbContext.Departments.Where(a => a.IsActive == true
                && (status.HasValue ? a.Status == status : true)
                && (!string.IsNullOrEmpty(search) ? a.Code.Contains(search) || a.Name.Contains(search) : true)
                && (fromDate.HasValue ? a.CreatedDate >= fromDate : true)
                && (toDate.HasValue ? a.CreatedDate <= toDate : true)
            );
            var count = await query.CountAsync();
            var list = await query.OrderByDescending(a => a.Id).Select(a => new GetListDepartmentDto
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
            var found = await _dbContext.Departments.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (found != null)
            {
                found.Status = !found.Status;
                await _dbContext.SaveChangesAsync();
            }

            return JsonResponse.Success(new { });
        }
    }
}