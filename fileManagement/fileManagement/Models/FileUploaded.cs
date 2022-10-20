namespace fileManagement.Models
{
    public class FileUploaded
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
        public DateTimeOffset CreationTime { get; set; } = DateTimeOffset.Now;
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeletionTime { get; set; }
    }
}
