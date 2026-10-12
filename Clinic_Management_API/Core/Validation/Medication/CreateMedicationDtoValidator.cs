using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Medication
{
    public class CreateMedicationDtoValidator : AbstractValidator<CreateMedicationDto>
    {
        public CreateMedicationDtoValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty().WithMessage("Name is Required")
                .MaximumLength(50).WithMessage("Name cannot exceed 50 characters");

            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is Required")
                .MaximumLength(200).WithMessage("Description cannot exceed 50 characters");

            RuleFor(m => m.StockQuantity).GreaterThan(0).WithMessage("Stock Quantity must be greater than 0");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero")
                .PrecisionScale(10,2,false);
        }
    }
}
