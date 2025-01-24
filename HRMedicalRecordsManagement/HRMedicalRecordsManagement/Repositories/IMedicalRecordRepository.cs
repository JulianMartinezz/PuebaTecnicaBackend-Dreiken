using HRMedicalRecordsManagement.Common.DeletionData;
using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.Models;

public interface IMedicalRecordRepository
{
    public void DetachEntity(TMedicalRecord entity);
    Task<IEnumerable<TMedicalRecord>> GetAllAsync();
    Task<TMedicalRecord> GetByIdAsync(int id);
    Task AddAsync(TMedicalRecord medicalRecord);
    Task UpdateAsync(TMedicalRecord medicalRecord);
    Task DeleteAsync(int id, DeletionData deletionData);
    Task<PagedList<TMedicalRecord>> GetFilteredMedicalRecordsAsync(
    int page,
    int pageSize,
    int? statusId,
    DateTime? startDate,
    DateTime? endDate,
    int? medicalRecordTypeId);
}