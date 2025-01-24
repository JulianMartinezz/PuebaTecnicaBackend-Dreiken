using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.Models;
using HRMedicalRecordsManagement.DTOs;

public interface IMedicalRecordService
{
    Task<IEnumerable<TMedicalRecord>> GetAllAsync();
    Task<TMedicalRecordDto> GetByIdAsync(int id);
    Task AddAsync(TMedicalRecord medicalRecord, string currentUser);
    Task UpdateAsync(TMedicalRecord medicalRecord, string currentUser);
    Task DeleteAsync(int id, string currentUser, string reason);
    Task<PagedList<TMedicalRecordDto>> GetFilteredMedicalRecordsAsync(
        int page,
        int pageSize,
        int? statusId,
        DateTime? startDate,
        DateTime? endDate,
        int? medicalRecordTypeId);
}