using yMoi.Dto;
using yMoi.Dto.Customer;
using yMoi.Dto.CustomerGroup;

namespace yMoi.Service.Interfaces
{
    public interface ICustomerService
    {
        Task<JsonResponseModel> GetListCustomer(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate);
        Task<JsonResponseModel> GetCustomerDetails(int id);
        Task<JsonResponseModel> GetCustomerByCode(string code);
        Task<JsonResponseModel> GetCustomerHistory(int id, int page, int limit);
        Task<JsonResponseModel> CreateCustomer(CreateCustomerModel dto, int createById);
        Task<JsonResponseModel> EditCustomer(int id, CreateCustomerModel dto, int updatedById);
        Task<JsonResponseModel> DeleteCustomer(int id);
        Task<JsonResponseModel> ToggleStatus(int id);
        Task<JsonResponseModel> GetListSearchCustomer(string? code, string? name, string? phone, DateTime? dob, string? nationality, string? identityCardNumber, int page = 1, int limit = 12);
        Task<JsonResponseModel> ImportCustomer(IFormFile file);
    }
}
