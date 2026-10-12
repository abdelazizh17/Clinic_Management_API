using Clinic_Management_API.Core.DTOs.MedicationDTOs;
using FluentValidation;

namespace Clinic_Management_API.Core.Validation.Medication
{
    public class UpdateMedicationDtoValidator : AbstractValidator<UpdateMedicationDto>
    {
        public UpdateMedicationDtoValidator()
        {
            RuleFor(m => m.Id).GreaterThan(0).WithMessage("Medication ID must be greater than 0");

            RuleFor(m => m.Name)
              .NotEmpty().WithMessage("Name is Required")
              .MaximumLength(50).WithMessage("Name cannot exceed 50 characters")
              .When(m => m.Name != null);

            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is Required")
                .MaximumLength(200).WithMessage("Description cannot exceed 50 characters")
                .When(m => m.Description != null);

            RuleFor(m => m.StockQuantity)
                .GreaterThan(0).WithMessage("Stock Quantity must be greater than 0")
                .When(m => m.StockQuantity != null);

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero")
                .PrecisionScale(10, 2, false)
                .When(m => m.Price != null);
        }
    }
}
