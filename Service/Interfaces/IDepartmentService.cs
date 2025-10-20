using yMoi.Dto;
using yMoi.Dto.Department;

namespace yMoi.Service.Interfaces
{
    public interface IDepartmentService
    {
        Task<JsonResponseModel> GetListDepartment(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate);
        Task<JsonResponseModel> GetDepartmentDetails(int id);
        Task<JsonResponseModel> CreateDepartment(CreateDepartmentDto dto, int createById);
        Task<JsonResponseModel> EditDepartment(int id, CreateDepartmentDto dto);
        Task<JsonResponseModel> DeleteDepartment(int id);
        Task<JsonResponseModel> ToggleStatus(int id);
    }
}