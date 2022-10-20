using AutoMapper;
using Azure.Storage.Blobs;
using fileManagement.Exceptions;
using fileManagement.Models;
using fileManagement.Models.Dtos;
using fileManagement.Repository;
using FluentValidation;

namespace fileManagement.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _blobStorageConnectionString;
        private readonly IFileUploadedRepository fileUploadedRepository;
        private readonly IMapper mapper;
        private readonly IValidator<FileUploadDto> fileUploadValidator;

        public BlobStorageService(IConfiguration configuration, IFileUploadedRepository fileUploadedRepository, IMapper mapper, IValidator<FileUploadDto> fileUploadValidator)
        {
            _blobStorageConnectionString = configuration.GetSection("AzureStorageAccount").GetValue<string>("BlobStorageConnectionString");
            this.fileUploadedRepository = fileUploadedRepository;
            this.mapper = mapper;
            this.fileUploadValidator = fileUploadValidator;
        }

        public async Task<ApiResponse<FileUploadedDto>> GetAll()
        {
            var allFiles = await fileUploadedRepository.GetAll(x => !x.IsDeleted);
            var filesMapped = mapper.Map<IEnumerable<FileUploadedDto>>(allFiles);
            var apiResponse = new ApiResponse<FileUploadedDto>();
            apiResponse.result = new ResultBody<FileUploadedDto> { items = filesMapped.ToList(), totalAcount = filesMapped.Count() };
            return apiResponse;
        }

        public async Task<ApiResponse<FileUploadedDto>> GetAllWithDeleted()
        {
            var allFiles = await fileUploadedRepository.GetAll();
            var filesMapped = mapper.Map<IEnumerable<FileUploadedDto>>(allFiles);
            var apiResponse = new ApiResponse<FileUploadedDto>();
            apiResponse.result = new ResultBody<FileUploadedDto> { items = filesMapped.ToList(), totalAcount = filesMapped.Count() };
            return apiResponse;
        }

        public async Task<ApiResponse<FileUploadedDto>> Upload(FileUploadDto uploadFile)
        {
            await fileUploadValidator.ValidateAndThrowAsync(uploadFile);
            var fileName = GetFileName(uploadFile.File);
            var container = new BlobContainerClient(_blobStorageConnectionString, uploadFile.BlobStorageContainer);
            var blob = container.GetBlobClient(fileName);

            using var stream = new MemoryStream();
            await uploadFile.File.CopyToAsync(stream);
            stream.Position = 0;
            var result = await blob.UploadAsync(stream);

            if (result.GetRawResponse().IsError)
                throw new BlobStorageException("An error occurred trying to save the file in Azure Blob Storage");

            var newFile = new FileUploaded
            {
                FileName = fileName,
                FileUrl = blob.Uri.ToString()
            };

            var fileCreated = mapper.Map<FileUploadedDto>(await fileUploadedRepository.Create(newFile));

            var apiResponse = new ApiResponse<FileUploadedDto>();
            var resultList = new List<FileUploadedDto>();
            resultList.Add(fileCreated);
            apiResponse.result = new ResultBody<FileUploadedDto> { items = resultList, totalAcount = 1 };
            return apiResponse;
        }

        public async Task<ApiResponse<FileUploadedDto>> Delete(int id, DeleteFileUploadedDto model)
        {
            if (id != model.Id) throw new BadRequestException("The IDs provided not macth");
            var file = await fileUploadedRepository.GetById(id);
            if (file == null) throw new NotFoundException($"A file with id {id} was not found");

            var container = new BlobContainerClient(_blobStorageConnectionString, model.BlobStorageContainer);
            var result = await container.GetBlobClient(file.FileName).DeleteIfExistsAsync();

            if (!result)
                throw new BlobStorageException("An error occurred trying to delete the file in Azure Blob Storage");

            file.IsDeleted = true;
            file.DeletionTime = DateTimeOffset.Now;
            await fileUploadedRepository.Delete(file);

            var apiResponse = new ApiResponse<FileUploadedDto>();
            apiResponse.result = new ResultBody<FileUploadedDto> { items = null, totalAcount = 0 };
            return apiResponse;
        }

        public Task<IEnumerable<FileUploadedDto>> Upload(List<FileUploadDto> uploadFile)
        {
            throw new NotImplementedException();
        }

        private string GetFileName(IFormFile file)
        {
            return $"{Path.GetFileNameWithoutExtension(file.FileName)}-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        }
    }
}
