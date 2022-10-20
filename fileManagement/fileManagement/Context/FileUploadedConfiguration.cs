using fileManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace fileManagement.Context
{
    public class FileUploadedConfiguration : IEntityTypeConfiguration<FileUploaded>
    {
        public void Configure(EntityTypeBuilder<FileUploaded> builder)
        {
            builder.Property(x => x.FileName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.FileUrl).IsRequired().HasMaxLength(300);
        }
    }
}
