using Microsoft.AspNetCore.Mvc;
using yMoi.Dto;
using yMoi.Dto.Upload;
using yMoi.Service.Interfaces;

namespace yMoi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController : ControllerBase
    {
        private readonly IUploadFileService _uploadService;

        public UploadController(IUploadFileService uploadService)
        {
            _uploadService = uploadService;
        }

        [HttpPost]
        public async Task<JsonResponseModel> UploadFiles([FromForm] UploadFilesDto dto)
        {
            return await _uploadService.UploadFiles(dto.Files);
        }

    }
}