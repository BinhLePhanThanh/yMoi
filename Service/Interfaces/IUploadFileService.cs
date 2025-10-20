using yMoi.Dto;

namespace yMoi.Service.Interfaces
{
    public interface IUploadFileService
    {
        Task<JsonResponseModel> UploadFiles(List<IFormFile> files);
    }
}