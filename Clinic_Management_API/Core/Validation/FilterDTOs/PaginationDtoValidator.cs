using Clinic_Management_API.Core.DTOs.FilterDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.FilterDTOs
{
    public class PaginationDtoValidator : AbstractValidator<PaginationDto>
    {
        public PaginationDtoValidator()
        {
            RuleFor(f => f.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(f => f.PageSize)
                .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
}
