using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using yMoi.Dto;
using yMoi.Dto.CustomerGroup;
using yMoi.Dto.Medicine;
using yMoi.Service;
using yMoi.Service.Interfaces;

namespace yMoi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerGroupController : ControllerBase
    {
        private readonly ICustomerGroupService _customerGroupService;

        public CustomerGroupController(ICustomerGroupService customerGroupService)
        {
            _customerGroupService = customerGroupService;
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResponseModel> GetListCustomerGroup(int page = 1, int limit = 12, bool? status = null, string? search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _customerGroupService.GetListCustomerGroup(page, limit, status, search, fromDate, toDate);
        }

        [HttpGet("{id}/customers")]
        [Authorize]
        public async Task<JsonResponseModel> GetCustomerGroupCustomers(int id, int page = 1, int limit = 12)
        {
            return await _customerGroupService.GetCustomerGroupCustomers(id, page, limit);
        }

        [HttpGet("{id}/histories")]
        [Authorize]
        public async Task<JsonResponseModel> GetCustomerGroupHistories(int id, int page = 1, int limit = 12)
        {
            return await _customerGroupService.GetCustomerGroupHistory(id, page, limit);
        }

        [HttpGet("{code}/code")]
        [Authorize]
        public async Task<JsonResponseModel> GetCustomerGroupDetails(string code)
        {
            return await _customerGroupService.GetCustomerGroupByCode(code);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetCustomerGroupDetails(int id)
        {
            return await _customerGroupService.GetCustomerGroupDetails(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResponseModel> CreateCustomerGroup(CreateCustomerGroupModel dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _customerGroupService.CreateCustomerGroup(dto, int.Parse(userId));
        }

        [HttpPost("AddCustomerToGroup")]
        [Authorize]
        public async Task<JsonResponseModel> AddCustomerToGroup(AddCustomerIdsModel dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _customerGroupService.AddCustomerToGroup(dto);
        }

        [HttpPost("RemoveCustomerFromGroup")]
        [Authorize]
        public async Task<JsonResponseModel> RemoveCustomerFromGroup(AddCustomerIdsModel dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _customerGroupService.RemoveCustomerFromGroup(dto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> EditCustomerGroup(int id, CreateCustomerGroupModel dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _customerGroupService.EditCustomerGroup(id, dto, int.Parse(userId));
        }

        [HttpPut("{id}/ToggleStatus")]
        [Authorize]
        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            return await _customerGroupService.ToggleStatus(id);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> DeleteCustomerGroup(int id)
        {
            return await _customerGroupService.DeleteCustomerGroup(id);
        }
    }
}
