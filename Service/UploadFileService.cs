using yMoi.Dto;
using yMoi.Service.Interfaces;
using yMoi.Util;
using Microsoft.AspNetCore.Hosting;

namespace yMoi.Service
{
    public class UploadFileService : IUploadFileService
    {
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;


        public UploadFileService(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<JsonResponseModel> UploadFiles(List<IFormFile> files)
        {
            List<dynamic> listImage = new List<dynamic>();

            var pathToSave = Path.Combine(_hostingEnvironment.ContentRootPath, "StaticFiles");

            foreach (var file in files)
            {
                string name = Guid.NewGuid() + "_" + DateTime.Now.ToString("ssddMMyyyy") + "_" + Utils.RandomString(6, true) + "_" + Path.GetExtension(file.FileName);
                var fullPath = Path.Combine(pathToSave, name);
                var url = "http://ymoi.runasp.net/static-files/" + name;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                listImage.Add(new
                {
                    name = file.FileName,
                    url = url,
                });
            }

            return JsonResponse.Success(listImage);
        }
    }
}