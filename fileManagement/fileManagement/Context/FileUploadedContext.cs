using fileManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace fileManagement.Context
{
    public class FileUploadedContext:DbContext
    {
        public DbSet<FileUploaded> FileUploaded { get; set; }
        public FileUploadedContext(DbContextOptions<FileUploadedContext> options) :base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new FileUploadedConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
