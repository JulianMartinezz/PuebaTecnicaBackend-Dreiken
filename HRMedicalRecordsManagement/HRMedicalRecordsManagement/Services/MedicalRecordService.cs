using AutoMapper;
using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.DTOs;
using HRMedicalRecordsManagement.Models;

namespace HRMedicalRecordsManagement.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;
    private readonly IMapper _mapper;

    public MedicalRecordService(IMedicalRecordRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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
        await _repository.AddAsync(medicalRecord);
    }

    public async Task UpdateAsync(TMedicalRecord medicalRecord)
    {
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