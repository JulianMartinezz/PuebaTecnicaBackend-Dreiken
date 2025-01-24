using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.DTOs;
using HRMedicalRecordsManagement.Models;

namespace HRMedicalRecordsManagement.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<TMedicalRecordDto> _validator;

    public MedicalRecordService(
        IMedicalRecordRepository repository, 
        IMapper mapper,
        IValidator<TMedicalRecordDto> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task<IEnumerable<TMedicalRecord>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

     public async Task<TMedicalRecordDto> GetByIdAsync(int id)
    {
        var medicalRecord = await _repository.GetByIdAsync(id);

        var recordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
        return recordDto;
    }

    public async Task AddAsync(TMedicalRecord medicalRecord)
    {
        //Mapping
        var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);

        //Validation
        ValidationResult validationResult = await _validator.ValidateAsync(medicalRecordDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        //Add validated TMedicalRecord
        await _repository.AddAsync(medicalRecord);
    }

    public async Task UpdateAsync(TMedicalRecord medicalRecord)
    {
        //Map
        var medicalRecordDto = _mapper.Map<TMedicalRecordDto>(medicalRecord);
        //Validation
        ValidationResult validationResult = await _validator.ValidateAsync(medicalRecordDto);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        //Update validated TMedicalRecord
        await _repository.UpdateAsync(medicalRecord);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
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