using Microsoft.AspNetCore.Mvc;
using yMoi.Dto;

namespace yMoi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilController : ControllerBase
    {
        public UtilController()
        {

        }

        [HttpGet("Languages")]
        public async Task<JsonResponseModel> GetLanguages()
        {
            return JsonResponse.Success(Constants.Contants.languages);
        }

        [HttpGet("Nationalities")]
        public async Task<JsonResponseModel> GetNationalities()
        {
            return JsonResponse.Success(Constants.Contants.nationalities);
        }

        [HttpGet("EducationLevels")]
        public async Task<JsonResponseModel> GetEducationLevels()
        {
            return JsonResponse.Success(Constants.Contants.educationLevels);
        }
    }
}