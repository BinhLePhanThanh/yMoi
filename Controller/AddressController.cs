
using System.Numerics;
using Microsoft.AspNetCore.Mvc;
using yMoi.Dto;
using yMoi.Service.Interfaces;

namespace yMoi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("ProvinceV2")]
        public async Task<JsonResponseModel> GetProvincesV2()
        {
            return await _addressService.GetProvinceV2();
        }

        [HttpGet("WardV2")]
        public async Task<JsonResponseModel> GetWardV2(int provinceId)
        {
            return await _addressService.GetWardV2(provinceId);
        }

        [HttpPost("SyncAddress")]
        public async Task<JsonResponseModel> SyncAddress()
        {
            await _addressService.SyncAddress();
            return JsonResponse.Success(new { });
        }
    }
}