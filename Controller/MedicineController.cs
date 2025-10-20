using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using yMoi.Dto;
using yMoi.Dto.Medicine;
using yMoi.Dto.Service;
using yMoi.Service.Interfaces;

namespace yMoi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineService _medicineService;
        public MedicineController(IMedicineService medicineService)
        {
            _medicineService = medicineService;
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResponseModel> GetListMedicine(int page = 1, int limit = 12, bool? status = null, string? search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _medicineService.GetListMedicine(page, limit, status, search, fromDate, toDate);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetMedicineDetails(int id)
        {
            return await _medicineService.GetMedicineDetails(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResponseModel> CreateMedicine(CreateMedicineDto dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _medicineService.CreateMedicine(dto, int.Parse(userId));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> EditMedicine(int id, CreateMedicineDto dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _medicineService.EditMedicine(id, dto);
        }

        [HttpPut("{id}/ToggleStatus")]
        [Authorize]
        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            return await _medicineService.ToggleStatus(id);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> DeleteMedicine(int id)
        {
            return await _medicineService.DeleteMedicine(id);
        }
    }
}
