using fileManagement.Models.Dtos;
using FluentValidation;

namespace fileManagement.Validations
{
    public class FileUploadValidator : AbstractValidator<FileUploadDto>
    {
        public FileUploadValidator()
        {
            RuleFor(x => x.File)
                    .NotEmpty()
                        .WithMessage("The file must not be empty")
                    .NotEmpty()
                        .WithMessage("The file must not be empty");

            RuleFor(x => x.BlobStorageContainer)
                    .NotEmpty()
                        .WithMessage("The BlobStorageContainer must not be empty")
                    .NotEmpty()
                        .WithMessage("The BlobStorageContainer must not be empty");
        }
    }
}
