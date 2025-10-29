using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using yMoi.Dto;
using yMoi.Dto.Customer;
using yMoi.Service.Interfaces;

namespace yMoi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpGet]
        [Authorize]
        public async Task<JsonResponseModel> GetListCustomer(int page = 1, int limit = 12, bool? status = null, string? search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _customerService.GetListCustomer(page, limit, status, search, fromDate, toDate);
        }

        [HttpGet("{id}/histories")]
        [Authorize]
        public async Task<JsonResponseModel> GetCustomerHistories(int id, int page = 1, int limit = 12)
        {
            return await _customerService.GetCustomerHistory(id, page, limit);
        }

        [HttpGet("{code}/code")]
        [Authorize]
        public async Task<JsonResponseModel> GetCustomerDetails(string code)
        {
            return await _customerService.GetCustomerByCode(code);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetCustomerDetails(int id)
        {
            return await _customerService.GetCustomerDetails(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResponseModel> CreateCustomer(CreateCustomerModel dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _customerService.CreateCustomer(dto, int.Parse(userId));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> EditCustomer(int id, CreateCustomerModel dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _customerService.EditCustomer(id, dto, int.Parse(userId));
        }

        [HttpPut("{id}/ToggleStatus")]
        [Authorize]
        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            return await _customerService.ToggleStatus(id);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> DeleteCustomer(int id)
        {
            return await _customerService.DeleteCustomer(id);
        }
    }
}
