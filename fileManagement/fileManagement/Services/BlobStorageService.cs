using AutoMapper;
using Azure.Storage.Blobs;
using fileManagement.Exceptions;
using fileManagement.Helpers;
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
        private readonly IFileHelper fileHelper;

        public BlobStorageService(IConfiguration configuration, IFileUploadedRepository fileUploadedRepository, IMapper mapper, IValidator<FileUploadDto> fileUploadValidator, IFileHelper fileHelper)
        {
            _blobStorageConnectionString = configuration.GetSection("AzureStorageAccount").GetValue<string>("BlobStorageConnectionString");

            this.fileUploadedRepository = fileUploadedRepository;
            this.mapper = mapper;
            this.fileUploadValidator = fileUploadValidator;
            this.fileHelper = fileHelper;
        }

        public async Task<ApiResponse<FileUploadedDto>> GetAll()
        {
            var allFiles = await fileUploadedRepository.GetAll(x => !x.IsDeleted);
            var filesMapped = mapper.Map<IEnumerable<FileUploadedDto>>(allFiles);
            var apiResponse = new ApiResponse<FileUploadedDto>();
            apiResponse.result = new ResultBody<FileUploadedDto> { items = filesMapped.ToList(), totalAcount = filesMapped.Count() };
            return apiResponse;
        }

        public async Task<ApiResponse<FileDownloadDto>> DownloadFile(int id, string blobStorageContainer)
        {
            var file = await fileUploadedRepository.GetById(id);
            if (file == null) throw new NotFoundException($"A file with id {id} was not found");

            var container = new BlobContainerClient(_blobStorageConnectionString, blobStorageContainer);

            var result = container.GetBlobClient(file.FileName);

            if (!await result.ExistsAsync())
                throw new BadRequestException($"The file {file.FileName} does not exist in Azure Blob Storage");

            using MemoryStream stream = new();
            await result.DownloadToAsync(stream);
            var content = Convert.ToBase64String(stream.ToArray());

            var fileDownLoad = new FileDownloadDto
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = content
            };

            var apiResponse = new ApiResponse<FileDownloadDto>();
            var resultList = new List<FileDownloadDto>();
            resultList.Add(fileDownLoad);
            apiResponse.result = new ResultBody<FileDownloadDto> { items = resultList, totalAcount = 1 };
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

            var fileName = fileHelper.GetFileName(uploadFile.File);
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
                FileUrl = blob.Uri.ToString(),
                ContentType = uploadFile.File.ContentType
            };

            var fileCreated = mapper.Map<FileUploadedDto>(await fileUploadedRepository.Create(newFile));

            var apiResponse = new ApiResponse<FileUploadedDto>();
            var resultList = new List<FileUploadedDto>();
            resultList.Add(fileCreated);
            apiResponse.result = new ResultBody<FileUploadedDto> { items = resultList, totalAcount = 1 };
            return apiResponse;
        }

        public async Task<ApiResponse<FileUploadedDto>> Upload(MultipleFileUploadDto uploadFile)
        {
            if (!uploadFile.Files.Any())
                throw new BadRequestException("Cannot send an empty files");

            if (uploadFile.Files.Count() > 5)
                throw new BadRequestException("The maximum files allowed are 5 per bacth");

            var container = new BlobContainerClient(_blobStorageConnectionString, uploadFile.BlobStorageContainer);

            List<FileUploaded> fileUploadeds = new();
            foreach (var file in uploadFile.Files)
            {
                var fileName = fileHelper.GetFileName(file);
                var blob = container.GetBlobClient(fileName);
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                stream.Position = 0;
                var result = await blob.UploadAsync(stream);

                if (result.GetRawResponse().IsError)
                    throw new BlobStorageException($"An error occurred trying to save the file {fileName} in Azure Blob Storage");

                var newFile = new FileUploaded
                {
                    FileName = fileName,
                    FileUrl = blob.Uri.ToString(),
                    ContentType = file.ContentType,
                };

                fileUploadeds.Add(newFile);
            }

            var filesCreated = mapper.Map<IEnumerable<FileUploadedDto>>(await fileUploadedRepository.Create(fileUploadeds));

            ApiResponse<FileUploadedDto> apiResponse = new();
            apiResponse.result = new ResultBody<FileUploadedDto> { items = filesCreated.ToList(), totalAcount = filesCreated.Count() };
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

            ApiResponse<FileUploadedDto> apiResponse = new();
            apiResponse.result = new ResultBody<FileUploadedDto> { items = null, totalAcount = 0 };
            return apiResponse;
        }
    }
}
