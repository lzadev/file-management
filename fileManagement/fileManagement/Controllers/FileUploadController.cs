using fileManagement.Helpers;
using fileManagement.Models;
using fileManagement.Models.Dtos;
using fileManagement.Services;
using Microsoft.AspNetCore.Mvc;

namespace fileManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadController : ControllerBase
    {
        private readonly IBlobStorageService blobStorage;
        private readonly IFileHelper fileHelper;

        public FileUploadController(IBlobStorageService blobStorage,IFileHelper fileHelper)
        {
            this.blobStorage = blobStorage;
            this.fileHelper = fileHelper;
        }

        [HttpGet("GetAll")]
        public async Task<ApiResponse<FileUploadedDto>> GetAll() => await blobStorage.GetAll();

        [HttpGet("DownloadFile/{id}")]
        public async Task<ApiResponse<FileDownloadDto>> DownloadFile(int id, string blobStorageContainer) => await blobStorage.DownloadFile(id, blobStorageContainer);

        [HttpGet("GetAllWithDeleted")]
        public async Task<ApiResponse<FileUploadedDto>> GetAllWithDeleted() => await blobStorage.GetAllWithDeleted();

        [HttpPost("ViewFile")]
        public FileContentResult ViewFile(FileViewDto content) => File(fileHelper.GetFileFromBase64(content.Content), content.ContentType);

        [HttpPost("UploadSingleFile")]
        public async Task<ApiResponse<FileUploadedDto>> Upload([FromForm] FileUploadDto model) => await blobStorage.Upload(model);

        [HttpPost("UplaodMultipleFiles")]
        public async Task<ApiResponse<FileUploadedDto>> Upload([FromForm] MultipleFileUploadDto model) => await blobStorage.Upload(model);

        [Route("DeleteUploadedFile/{id}")]
        [HttpDelete]
        public async Task<ApiResponse<FileUploadedDto>> Delete(int id,DeleteFileUploadedDto model) => await blobStorage.Delete(id,model);
    }
}
