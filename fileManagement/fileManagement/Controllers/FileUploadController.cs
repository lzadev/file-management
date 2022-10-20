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

        public FileUploadController(IBlobStorageService blobStorage)
        {
            this.blobStorage = blobStorage;
        }

        [HttpPost("Upload")]
        public async Task<ApiResponse<FileUploadedDto>> Upload([FromForm] FileUploadDto model) => await blobStorage.Upload(model);

        [HttpGet("GetAll")]
        public async Task<ApiResponse<FileUploadedDto>> GetAll() => await blobStorage.GetAll();

        [HttpGet("GetAllWithDeleted")]
        public async Task<ApiResponse<FileUploadedDto>> GetAllWithDeleted() => await blobStorage.GetAllWithDeleted();

        [Route("DeleteUploadedFile/{id}")]
        [HttpDelete]
        public async Task<ApiResponse<FileUploadedDto>> Delete(int id,DeleteFileUploadedDto model) => await blobStorage.Delete(id,model);
    }
}
