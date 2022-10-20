using fileManagement.Models;
using fileManagement.Models.Dtos;

namespace fileManagement.Services
{
    public interface IBlobStorageService
    {
        Task<ApiResponse<FileUploadedDto>> Upload(FileUploadDto uploadFile);
        Task<IEnumerable<FileUploadedDto>> Upload(List<FileUploadDto> uploadFile);
        Task<ApiResponse<FileUploadedDto>> GetAll();
        Task<ApiResponse<FileUploadedDto>> GetAllWithDeleted();
        Task<ApiResponse<FileUploadedDto>> Delete(int id, DeleteFileUploadedDto model);
    }
}
