namespace fileManagement.Models.Dtos
{
    public class FileUploadedDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public string ContentType { get; set; }
    }
}
