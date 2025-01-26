using FluentValidation;
using HRMedicalRecordsManagement.DTOs;

namespace HRMedicalRecordsManagement.Validators;

public class TMedicalRecordValidator : AbstractValidator<TMedicalRecordDto>
{
    private readonly IStatusRepository _statusRepository;
    private readonly IMedicalRecordTypeRepository _medicalRecordTypeRepository;
    public TMedicalRecordValidator(
        IStatusRepository statusRepository,
        IMedicalRecordTypeRepository medicalRecordTypeRepository)
    {
        _statusRepository = statusRepository ?? throw new ArgumentNullException(nameof(statusRepository));
        _medicalRecordTypeRepository = medicalRecordTypeRepository ?? throw new ArgumentNullException(nameof(medicalRecordTypeRepository));

        //2.1 Date Controls Validations
        RuleFor(x => x.StartDate)
            .NotNull().WithMessage("START_DATE is required")
            .Must((model, startDate) => startDate <= model.EndDate)
                .When(x => x.StatusId != 1 && x.EndDate.HasValue)
                .WithMessage("START_DATE cannot be later than END_DATE")
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
                .When(x => x.StartDate.HasValue)
                .WithMessage("START_DATE cannot be a future date");

        // End Date validation
        RuleFor(x => x.EndDate)
            .GreaterThanOrEqualTo(x => x.StartDate).When(x => x.StartDate.HasValue && x.StatusId != 1).WithMessage("If END_DATE exists, it must be later than START_DATE")
            .When(x => x.StatusId != 1)
            .WithMessage("END_DATE is required when Status is not Active");

        
        //2.2 Required fields
        RuleFor(x => x.Diagnosis)
            .NotEmpty().WithMessage("DIAGNOSIS is required");
        
        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("START_DATE is required");
        
        RuleFor(x => x.StatusId)
            .NotEmpty().WithMessage("STATUS_ID is required");
        
        RuleFor(x => x.MedicalRecordTypeId)
            .NotEmpty().WithMessage("MEDICAL_RECORD_TYPE_ID is required");

        RuleFor(x => x.FileId)
            .NotEmpty().WithMessage("FILE_ID is required");

        RuleFor(x => x.CreatedBy)
            .NotEmpty().WithMessage("CREATED_BY is required");

        //2.3 Related Records Validation
        RuleFor(x => x.StatusId)
            .MustAsync(async (statusId, cancellation) => await _statusRepository.StatusExistsAsync(statusId))
                .WithMessage("STATUS_ID must exist in the STATUS table");
        
        RuleFor(x => x.MedicalRecordTypeId)
            .MustAsync(async (typeId, cancellation) => await _medicalRecordTypeRepository.MedicalRecordTypeExistsAsync(typeId)).WithMessage("MEDICAL_RECORD_TYPE_ID must exist in the MEDICAL_RECORD_TYPE table");

        //Check with flag
        RuleFor(x => x.StatusId)
            .NotEqual(2).When(x => x.StatusId == 0).WithMessage("Cannot assign 'Inactive' status when creating a new record")
            .When(x => !x.isUpdate);

        RuleFor(x => x.DeletionReason)
            .NotEmpty().When(x => x.StatusId == 2).WithMessage("To change to 'Inactive' status, DELETION_REASON must be provided");
        
        //2.4 Maximum Length Validation
        RuleFor(x => x.Diagnosis)
            .MaximumLength(100).WithMessage("DIAGNOSIS should not exceed 100 characters");

        RuleFor(x => x.MotherData)
            .MaximumLength(2000).WithMessage("MOTHER_DATA should not exceed 2000 characters");

        RuleFor(x => x.FatherData)
            .MaximumLength(2000).WithMessage("FATHER_DATA should not exceed 2000 characters");

        RuleFor(x => x.OtherFamilyData)
            .MaximumLength(2000).WithMessage("OTHER_FAMILY_DATA should not exceed 2000 characters");

        RuleFor(x => x.MedicalBoard)
            .MaximumLength(200).WithMessage("MEDICAL_BOARD should not exceed 200 characters");

        RuleFor(x => x.DeletionReason)
            .MaximumLength(2000).WithMessage("DELETION_REASON should not exceed 2000 characters");

        RuleFor(x => x.Observations)
            .MaximumLength(2000).WithMessage("OBSERVATIONS should not exceed 2000 characters");

        RuleFor(x => x.Audiometry)
            .Must(val => val == null || val == "Y" || val == "N")
            .WithMessage("If AUDIOMETRY exists, it must be 'Y' or 'N'");
        
        RuleFor(x => x.PositionChange)
            .Must(val => val == null || val == "Y" || val == "N")
            .WithMessage("If POSITION_CHANGE exists, it must be 'Y' or 'N'");
        
        RuleFor(x => x.ExecuteMicros)
            .Must(val => val == null || val == "Y" || val == "N")
            .WithMessage("If EXECUTE_MICROS exists, it must be 'Y' or 'N'");
        RuleFor(x => x.ExecuteExtra)
            .Must(val => val == null || val == "Y" || val == "N")
            .WithMessage("If EXECUTE_EXTRA exists, it must be 'Y' or 'N'");
        
        RuleFor(x => x.VoiceEvaluation)
            .Must(val => val == null || val == "Y" || val == "N")
            .WithMessage("If VOICE_EVALUATION exists, it must be 'Y' or 'N'");
        
        RuleFor(x => x.Disability)
            .Must(val => val == null || val == "Y" || val == "N")
            .WithMessage("If DISABILITY exists, it must be 'Y' or 'N'");
        
        RuleFor(x => x.AreaChange)
            .Must(val => val == null || val == "Y" || val == "N")
            .WithMessage("If AREA_CHANGE exists, it must be 'Y' or 'N'");

        // Status Control - 2.5
        RuleFor(x => x.StatusId)
            .Must(statusId => statusId == 1 || statusId == 2)
            .WithMessage("Status must be either Active (1) or Inactive (2).");

        // Active Status (ID: 1) Validation
        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("CREATED_BY is required for new records.")
            .When(x => x.StatusId == 1);

        // Inactive Status (ID: 2) Validation
        RuleFor(x => x.DeletionReason)
            .NotEmpty()
            .WithMessage("DELETION_REASON is required when status is 'Inactive'.")
            .When(x => x.StatusId == 2);

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("END_DATE is required when status is 'Inactive'.")
            .When(x => x.StatusId == 2);

        RuleFor(x => x.DeletedBy)
            .NotEmpty()
            .WithMessage("DELETED_BY is required when status is 'Inactive'.")
            .When(x => x.StatusId == 2);

        // Additional Validation Rules - 2.6

        RuleFor(x => x.DisabilityPercentage)
            .NotNull()
            .InclusiveBetween(0, 100)
            .When(x => x.Disability == "Y")
            .WithMessage("Disability Percentage must be between 0 and 100 when Disability is Y.");
            

        RuleFor(x => x.Observations)
            .NotEmpty()
            .When(x => x.PositionChange == "Y")
            .WithMessage("Observations are required when Position Change is Y.");

        
        RuleFor(x => x.StatusId)
            .Equal(2)
            .When(x => x.EndDate != null)
            .WithMessage("If End Date exists, status must be set to Inactive (ID: 2).");

        RuleFor(x => x.ModificationDate)
            .NotEmpty()
            .When(x => x.isUpdate)
            .WithMessage("Modification Date must be updated when a record is modified.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("Created By is required.");

        RuleFor(x => x.ModifiedBy)
            .NotEmpty()
            .WithMessage("Modified By is required.")
            .When(x => x.isUpdate);

        RuleFor(x => x.DeletedBy)
            .NotEmpty()
            .When(x => x.StatusId == 2)
            .WithMessage("Deleted By is required.");

    }
}