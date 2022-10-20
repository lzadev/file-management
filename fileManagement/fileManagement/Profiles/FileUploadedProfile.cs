using AutoMapper;
using fileManagement.Models;
using fileManagement.Models.Dtos;

namespace fileManagement.Profiles
{
    public class FileUploadedProfile : Profile
    {
        public FileUploadedProfile()
        {
            CreateMap<FileUploaded, FileUploadedDto>();
            CreateMap<FileUploadDto, FileUploaded>();
        }
    }
}
