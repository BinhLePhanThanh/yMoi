using yMoi.Dto;
using yMoi.Dto.Unit;

namespace yMoi.Service.Interfaces
{
    public interface IUnitService
    {
        Task<JsonResponseModel> GetListUnit(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate);
        Task<JsonResponseModel> GetUnitDetails(int id);
        Task<JsonResponseModel> CreateUnit(CreateUnitDto dto, int createById);
        Task<JsonResponseModel> EditUnit(int id, CreateUnitDto dto);
        Task<JsonResponseModel> DeleteUnit(int id);
        Task<JsonResponseModel> ToggleStatus(int id);
    }
}