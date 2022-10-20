namespace fileManagement.Services
{
    using fileManagement.Models;
    using fileManagement.Models.Dtos;
    public interface IBlobStorageService
    {
        Task<ApiResponse<FileUploadedDto>> Upload(FileUploadDto uploadFile);
        Task<ApiResponse<FileUploadedDto>> Upload(MultipleFileUploadDto uploadFile);
        Task<ApiResponse<FileUploadedDto>> GetAll();
        Task<ApiResponse<FileDownloadDto>> DownloadFile(int id, string blobStorageContainer);
        Task<ApiResponse<FileUploadedDto>> GetAllWithDeleted();
        Task<ApiResponse<FileUploadedDto>> Delete(int id, DeleteFileUploadedDto model);
    }
}
