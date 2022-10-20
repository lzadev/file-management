using fileManagement.Models;

namespace fileManagement.Repository
{
    public interface IFileUploadedRepository
    {
        Task<FileUploaded> Create(FileUploaded model);
        Task<List<FileUploaded>> Create(List<FileUploaded> model);
        Task<bool> Delete(FileUploaded model);
        Task<IEnumerable<FileUploaded>> GetAll();
        Task<IEnumerable<FileUploaded>> GetAll(Func<FileUploaded,bool> func);
        Task<FileUploaded> GetById(int id);
    }
}
