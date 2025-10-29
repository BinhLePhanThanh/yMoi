using yMoi.Dto;
using yMoi.Dto.CustomerGroup;
using yMoi.Dto.Department;

namespace yMoi.Service.Interfaces
{
    public interface ICustomerGroupService
    {
        Task<JsonResponseModel> GetListCustomerGroup(int page, int limit, bool? status, string? search, DateTime? fromDate, DateTime? toDate);
        Task<JsonResponseModel> GetCustomerGroupDetails(int id);
        Task<JsonResponseModel> GetCustomerGroupByCode(string code);
        Task<JsonResponseModel> GetCustomerGroupHistory(int id, int page, int limit);
        Task<JsonResponseModel> CreateCustomerGroup(CreateCustomerGroupModel dto, int createById);
        Task<JsonResponseModel> EditCustomerGroup(int id, CreateCustomerGroupModel dto, int updatedById);
        Task<JsonResponseModel> DeleteCustomerGroup(int id);
        Task<JsonResponseModel> ToggleStatus(int id);
        Task<JsonResponseModel> GetCustomerGroupCustomers(int id, int page, int limit);
        Task<JsonResponseModel> AddCustomerToGroup(AddCustomerIdsModel dto);
        Task<JsonResponseModel> RemoveCustomerFromGroup(AddCustomerIdsModel dto);


    }
}
