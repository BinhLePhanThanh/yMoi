using yMoi.Dto;
using yMoi.Dto.Medicine;
using yMoi.Dto.Service;

namespace yMoi.Service.Interfaces
{
    public interface IMedicineService
    {
        Task<JsonResponseModel> GetListMedicine(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate);
        Task<JsonResponseModel> GetMedicineDetails(int id);
        Task<JsonResponseModel> CreateMedicine(CreateMedicineDto dto, int createById);
        Task<JsonResponseModel> EditMedicine(int id, CreateMedicineDto dto);
        Task<JsonResponseModel> DeleteMedicine(int id);
        Task<JsonResponseModel> ToggleStatus(int id);
    }
}
