using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HRMedicalRecordsManagement.Common.DeletionData;
using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.DTOs;
using HRMedicalRecordsManagement.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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

     public async Task<TMedicalRecordDto> GetByIdAsync(int id)
    {
        var medicalRecord = await _repository.GetByIdAsync(id);
        var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
        
        return medicalRecordDto;
    }

    public async Task AddAsync(TMedicalRecord medicalRecord, string currentUser)
    {
        if (medicalRecord.StatusId==2){
            throw new InvalidOperationException("Can't create and delete a record at the same time");
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
            throw new ValidationException(validationResult.Errors);
        }

        //Add validated TMedicalRecord
        await _repository.AddAsync(medicalRecord);
    }

    public async Task UpdateAsync(TMedicalRecord medicalRecord, string currentUser)
    {
        //Call repository to check if it exists
        TMedicalRecord originalMedicalRecord = await _repository.GetByIdAsync(medicalRecord.MedicalRecordId) 
        ?? throw new KeyNotFoundException($"Record with ID {medicalRecord.MedicalRecordId} not found.");
        // Hand 'inactive' status
        if (originalMedicalRecord.StatusId == 2){
            throw new InvalidOperationException("Modifications are not allowed for records with 'Inactive' status");
        }
        else if (medicalRecord.StatusId == 2){
            throw new InvalidOperationException("Deletions are made with a Delete request");
        }

        _repository.DetachEntity(originalMedicalRecord);
        
        //Log user
        medicalRecord.ModifiedBy = currentUser;
        medicalRecord.ModificationDate = DateOnly.FromDateTime(DateTime.Now);

        //Map both
        var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
        
        //IsUpdate flag true
        medicalRecordDto.isUpdate = true;

        //Validation
        ValidationResult validationResult = await _validator.ValidateAsync(medicalRecordDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        
        
        //Update validated TMedicalRecord
         await _repository.UpdateAsync(medicalRecord);
        
    }

    public async Task DeleteAsync(int id, string currentUser, string reason)
    {
        //Log person
        var deletionDate = DateOnly.FromDateTime(DateTime.Now);
        var deletionData = new DeletionData(reason, currentUser, deletionDate);
        //Validate DeletionData
        var validationResult = await _deletionDataValidator.ValidateAsync(deletionData);
        if (!validationResult.IsValid)
        {
            throw new ArgumentException(string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage)));
        }
        //Act
        else await _repository.DeleteAsync(id, deletionData);
    }

    public async Task<PagedList<TMedicalRecordDto>> GetFilteredMedicalRecordsAsync(
         int page, 
        int pageSize, 
        int? statusId, 
        DateTime? startDate, 
        DateTime? endDate, 
        int? medicalRecordTypeId)
    {
        var medicalRecords = await _repository.GetFilteredMedicalRecordsAsync(
            page, pageSize, statusId, startDate, endDate, medicalRecordTypeId);
        
        //Map to DTO
        var medicalRecordDtos = _mapper.Map<IEnumerable<TMedicalRecordDto>>(medicalRecords.Items);

        return new PagedList<TMedicalRecordDto>(medicalRecordDtos, medicalRecords.TotalCount, page, pageSize);
    }
}