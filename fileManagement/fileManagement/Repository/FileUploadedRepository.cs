using fileManagement.Context;
using fileManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace fileManagement.Repository
{
    public class FileUploadedRepository : IFileUploadedRepository
    {
        private readonly FileUploadedContext context;

        public FileUploadedRepository(FileUploadedContext context)
        {
            this.context = context;
        }
        public async Task<FileUploaded> Create(FileUploaded model)
        {
            await context.AddAsync(model);
            await context.SaveChangesAsync();
            return model;
        }

        public async Task<bool> Delete(FileUploaded model)
        {
            context.FileUploaded.Update(model);
            return context.SaveChanges() > 0;
        }

        public async Task<IEnumerable<FileUploaded>> GetAll()
        {
            return await context.FileUploaded.ToListAsync();
        }

        public Task<IEnumerable<FileUploaded>> GetAll(Func<FileUploaded, bool> func)
        {
            return Task.FromResult(Task.Run(()=>context.FileUploaded.Where(func)).Result);
        }

        public async Task<FileUploaded> GetById(int id)
        {
            return await context.FileUploaded.FindAsync(id);
        }
    }
}
