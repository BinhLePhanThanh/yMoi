using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using yMoi.Dto;
using yMoi.Dto.Unit;
using yMoi.Service.Interfaces;

namespace yMoi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UnitController : ControllerBase
    {
        private readonly IUnitService _unitService;

        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        [HttpGet]
        [Authorize]
        public async Task<JsonResponseModel> GetListUnit(int page = 1, int limit = 12, bool? status = null, string? search = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _unitService.GetListUnit(page, limit, status, search, fromDate, toDate);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> GetUnitDetails(int id)
        {
            return await _unitService.GetUnitDetails(id);
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResponseModel> CreateUnit(CreateUnitDto dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);

            return await _unitService.CreateUnit(dto, int.Parse(userId));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> EditUnit(int id, CreateUnitDto dto)
        {
            var currentUser = HttpContext.User;
            string userId = currentUser.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _unitService.EditUnit(id, dto);
        }

        [HttpPut("{id}/ToggleStatus")]
        [Authorize]
        public async Task<JsonResponseModel> ToggleStatus(int id)
        {
            return await _unitService.ToggleStatus(id);
        }


        [HttpDelete("{id}")]
        [Authorize]
        public async Task<JsonResponseModel> DeleteUnit(int id)
        {
            return await _unitService.DeleteUnit(id);
        }
    }
}