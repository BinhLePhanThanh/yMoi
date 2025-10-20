using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using yMoi.Dto;
using yMoi.Dto.Department;
using yMoi.Service.Interfaces;

namespace yMoi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResponseModel> GetListDepartment(int page = 1, int limit = 12, bool? status = null, string? search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _departmentService.GetListDepartment(page, limit, status, search, fromDate, toDate);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetDepartmentDetails(int id)
        {
            return await _departmentService.GetDepartmentDetails(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResponseModel> CreateDepartment(CreateDepartmentDto dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _departmentService.CreateDepartment(dto, int.Parse(userId));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> EditDepartment(int id, CreateDepartmentDto dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _departmentService.EditDepartment(id, dto);
        }

        [HttpPut("{id}/ToggleStatus")]
        [Authorize]
        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            return await _departmentService.ToggleStatus(id);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> DeleteDepartment(int id)
        {
            return await _departmentService.DeleteDepartment(id);
        }
    }
}