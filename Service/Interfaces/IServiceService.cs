using yMoi.Dto;
using yMoi.Dto.Service;

namespace yMoi.Service.Interfaces
{
    public interface IServiceService
    {
        Task<JsonResponseModel> GetListService(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate);
        Task<JsonResponseModel> GetServiceDetails(int id);
        Task<JsonResponseModel> CreateService(CreateServiceDto dto, int createById);
        Task<JsonResponseModel> EditService(int id, CreateServiceDto dto);
        Task<JsonResponseModel> DeleteService(int id);
        Task<JsonResponseModel> ToggleStatus(int id);
    }
}