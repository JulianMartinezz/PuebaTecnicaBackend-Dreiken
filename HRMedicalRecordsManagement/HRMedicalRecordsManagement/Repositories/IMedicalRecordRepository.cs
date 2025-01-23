using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.Models;

public interface IMedicalRecordRepository
{
    Task<IEnumerable<TMedicalRecord>> GetAllAsync();
    Task<TMedicalRecord> GetByIdAsync(int id);
    Task AddAsync(TMedicalRecord medicalRecord);
    Task UpdateAsync(TMedicalRecord medicalRecord);
    Task DeleteAsync(int id);
    Task<PagedList<TMedicalRecord>> GetFilteredMedicalRecordsAsync(
    int page,
    int pageSize,
    int? statusId,
    DateTime? startDate,
    DateTime? endDate,
    int? medicalRecordTypeId);
}