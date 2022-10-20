using fileManagement.Exceptions;

namespace fileManagement.Helpers
{
    public class FileHelper : IFileHelper
    {
        public string GetFileName(IFormFile file)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            return $"{Path.GetFileNameWithoutExtension(file.FileName)}-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        }

        public byte[] GetFileFromBase64(string content) => Convert.FromBase64String(content);
    }

    public interface IFileHelper
    {
        string GetFileName(IFormFile file);
        byte[] GetFileFromBase64(string content);
    }
}
