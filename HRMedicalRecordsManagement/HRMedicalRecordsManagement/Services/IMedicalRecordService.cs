using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.Models;
using HRMedicalRecordsManagement.DTOs;

public interface IMedicalRecordService
{
    Task<IEnumerable<TMedicalRecord>> GetAllAsync();
    Task<TMedicalRecordDto> GetByIdAsync(int id);
    Task AddAsync(TMedicalRecord medicalRecord);
    Task UpdateAsync(TMedicalRecord medicalRecord);
    Task DeleteAsync(int id);
    Task<PagedList<TMedicalRecordDto>> GetFilteredMedicalRecordsAsync(
        int page,
        int pageSize,
        int? statusId,
        DateTime? startDate,
        DateTime? endDate,
        int? medicalRecordTypeId);
}