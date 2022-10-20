namespace fileManagement.Models.Dtos
{
    public class MultipleFileUploadDto
    {
        public List<IFormFile> Files { get; set; }
        public string BlobStorageContainer { get; set; }
    }
}
