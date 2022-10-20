using fileManagement.Context;
using fileManagement.Filters;
using fileManagement.Models.Dtos;
using fileManagement.Repository;
using fileManagement.Services;
using fileManagement.Validations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<FileUploadedContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddScoped<IValidator<FileUploadDto>, FileUploadValidator>();



builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddTransient<IBlobStorageService, BlobStorageService>();
builder.Services.AddTransient<IFileUploadedRepository, FileUploadedRepository>();



builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
