using HRMedicalRecordsManagement.Models;

public interface IMedicalRecordService
{
    Task<IEnumerable<TMedicalRecord>> GetAllAsync();
    Task<TMedicalRecord> GetByIdAsync(int id);
    Task AddAsync(TMedicalRecord medicalRecord);
    Task UpdateAsync(TMedicalRecord medicalRecord);
    Task DeleteAsync(int id);
}