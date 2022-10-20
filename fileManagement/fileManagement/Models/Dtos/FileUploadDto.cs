namespace fileManagement.Models.Dtos
{
    public class FileUploadDto
    {
        public IFormFile File { get; set; }
        public string BlobStorageContainer { get; set; }
    }
}
