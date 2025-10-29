using yMoi.Dto;

namespace yMoi.Service.Interfaces
{
    public interface IAddressService
    {
        Task<JsonResponseModel> GetProvinceV2();
        Task<JsonResponseModel> GetWardV2(int provinceId);
        Task<JsonResponseModel> SyncAddress();
    }
}