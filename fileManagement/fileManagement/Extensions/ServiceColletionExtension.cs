namespace fileManagement.Extensions
{
    using fileManagement.Context;
    using fileManagement.Filters;
    using fileManagement.Helpers;
    using fileManagement.Models.Dtos;
    using fileManagement.Repository;
    using fileManagement.Services;
    using fileManagement.Validations;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;
    public static class ServiceColletionExtension
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<FileUploadedContext>(options => options.UseSqlServer(configuration.GetConnectionString("db")));
            services.AddScoped<IValidator<FileUploadDto>, FileUploadValidator>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddTransient<IFileUploadedRepository, FileUploadedRepository>();
            services.AddTransient<IFileHelper, FileHelper>();

            services.AddControllers(options =>
            {
                options.Filters.Add<GlobalExceptionFilter>();
            });

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        }
    }
}
