using FluentValidation;
using HRMedicalRecordsManagement.Common.DeletionData;

public class DeletionDataValidator : AbstractValidator<DeletionData>
{
    public DeletionDataValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Deletion reason is required.");
        
        RuleFor(x => x.CurrentUser)
            .NotEmpty().WithMessage("User performing the deletion is required.");
        
        RuleFor(x => x.CurrentDate)
            .NotEqual(default(DateOnly)).WithMessage("Deletion date is required.");
    }
}