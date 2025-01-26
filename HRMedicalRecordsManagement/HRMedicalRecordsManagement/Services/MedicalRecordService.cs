using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HRMedicalRecordsManagement.Common.DeletionData;
using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.DTOs;
using HRMedicalRecordsManagement.Models;
using HRMedicalRecordsManagement.Helpers;
using HRMedicalRecordsManagement.Models.BaseResponse;

namespace HRMedicalRecordsManagement.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<TMedicalRecordDto> _validator;
    private readonly IValidator<DeletionData> _deletionDataValidator;

    public MedicalRecordService(
        IMedicalRecordRepository repository, 
        IMapper mapper,
        IValidator<TMedicalRecordDto> validator,
        IValidator<DeletionData> deletionDataValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _deletionDataValidator = deletionDataValidator;
    }

    public async Task<IEnumerable<TMedicalRecord>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

     public async Task<BaseResponse<TMedicalRecordDto>> GetByIdAsync(int id)
    {
        try
        {
            var medicalRecord = await _repository.GetByIdAsync(id);
            var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
            return ResponseHelper.Success(medicalRecordDto, "Medical record retrieved succesfully");
        }
        catch (KeyNotFoundException ex)
        {
            return ResponseHelper.NotFound<TMedicalRecordDto>(ex.Message);
        }

        catch (Exception ex)
        {
            return ResponseHelper.InternalServerError<TMedicalRecordDto>("An unexpected error ocurred", ex.Message);
        }
    }

    public async Task<BaseResponse<TMedicalRecordDto>> AddAsync(TMedicalRecord medicalRecord, string currentUser)
    {
        if (medicalRecord.StatusId==2){
            return ResponseHelper.BadRequest<TMedicalRecordDto>("StatusID can't be 2 when adding or modifying");
        }
        //Log user
        medicalRecord.CreatedBy = currentUser;
        medicalRecord.CreationDate = DateOnly.FromDateTime(DateTime.Now);
        //Mapping
        var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
        //IsUpdate flag false
        medicalRecordDto.isUpdate = false;
        //Validation
        ValidationResult validationResult = await _validator.ValidateAsync(medicalRecordDto);
        if (!validationResult.IsValid)
        {
            return ResponseHelper.BadRequest<TMedicalRecordDto>("Validation failed", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }
        //Add validated TMedicalRecord
        await _repository.AddAsync(medicalRecord);

        // Pass id of newly added record
        medicalRecordDto.MedicalRecordId = medicalRecord.MedicalRecordId;

        return ResponseHelper.Success(medicalRecordDto, "Medical record added successfully");
    }

    public async Task<BaseResponse<TMedicalRecordDto>> UpdateAsync(TMedicalRecord medicalRecord, string currentUser)
    {
        //Call repository to check if it exists
        try
        {
            TMedicalRecord originalMedicalRecord = await _repository.GetByIdAsync(medicalRecord.MedicalRecordId);

            // Hand 'inactive' status
            if (originalMedicalRecord.StatusId == 2){
                return ResponseHelper.BadRequest<TMedicalRecordDto>("Modifications are not allowed for records with 'Inactive' status");
            }
            else if (medicalRecord.StatusId == 2){
                return ResponseHelper.BadRequest<TMedicalRecordDto>("Deletions are made with the Delete request");
            }

            _repository.DetachEntity(originalMedicalRecord);

            //Log user
            medicalRecord.ModifiedBy = currentUser;
            medicalRecord.ModificationDate = DateOnly.FromDateTime(DateTime.Now);

            //Map
            var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
            
            //IsUpdate flag true
            medicalRecordDto.isUpdate = true;

            //Validation
            ValidationResult validationResult = await _validator.ValidateAsync(medicalRecordDto);
            if (!validationResult.IsValid)
            {
                return ResponseHelper.BadRequest<TMedicalRecordDto>("Validation failed", string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
            }
            
            //Update validated TMedicalRecord
            await _repository.UpdateAsync(medicalRecord);
            return ResponseHelper.Success<TMedicalRecordDto>(medicalRecordDto, "Medical record was modified successfully");

        }
        catch (KeyNotFoundException ex)
        {
            return ResponseHelper.NotFound<TMedicalRecordDto>(ex.Message);
        }

        catch (Exception ex)
        {
            return ResponseHelper.InternalServerError<TMedicalRecordDto>("An unexpected error ocurred", ex.Message);
        }
    }

    public async Task<BaseResponse<TMedicalRecordDto>> DeleteAsync(int id, string currentUser, string reason)
    {
        //Log person
        var deletionDate = DateOnly.FromDateTime(DateTime.Now);
        var deletionData = new DeletionData(reason, currentUser, deletionDate);
        //Validate DeletionData
        var validationResult = await _deletionDataValidator.ValidateAsync(deletionData);
        if (!validationResult.IsValid)
        {
            return ResponseHelper.BadRequest<TMedicalRecordDto>("Validation failed" , string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }
        //Act
        else 
        {
            try 
            {
                await _repository.DeleteAsync(id, deletionData);
                var medicalRecord = await _repository.GetByIdAsync(id);
                //MAP
                var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
                return ResponseHelper.Success(medicalRecordDto , "Medical record successfully set to inactive");
            }
            catch (KeyNotFoundException ex)
            {
                return ResponseHelper.NotFound<TMedicalRecordDto>(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return ResponseHelper.BadRequest<TMedicalRecordDto>(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseHelper.InternalServerError<TMedicalRecordDto>("An unexpected error ocurred", ex.Message);
            }
        }
    }

    public async Task<BaseResponse<PagedList<TMedicalRecordDto>>> GetFilteredMedicalRecordsAsync(
         int page, 
        int pageSize, 
        int? statusId, 
        DateTime? startDate, 
        DateTime? endDate, 
        int? medicalRecordTypeId)
    {
        if (page < 1 || pageSize < 1)
        {
            return ResponseHelper.BadRequest<PagedList<TMedicalRecordDto>>("Page and PageSize must be greater than 0");
        }

        var medicalRecords = await _repository.GetFilteredMedicalRecordsAsync(
            page, pageSize, statusId, startDate, endDate, medicalRecordTypeId);
        
        if (medicalRecords.TotalPages < medicalRecords.CurrentPage)
        {
            return ResponseHelper.BadRequest<PagedList<TMedicalRecordDto>>("Page is higher than the number of pages");
        }

        var totalRows = medicalRecords.TotalCount;
        
        //Map to DTO
        var medicalRecordDtos = _mapper.Map<IEnumerable<TMedicalRecordDto>>(medicalRecords.Items);

        var pagedMedicalRecordDtos = new PagedList<TMedicalRecordDto>(
            medicalRecordDtos,
            totalRows,
            page,
            pageSize
            );

        return ResponseHelper.Success(pagedMedicalRecordDtos, "Request was succesful", totalRows);
    }
}