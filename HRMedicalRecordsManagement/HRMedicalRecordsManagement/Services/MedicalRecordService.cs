using HRMedicalRecordsManagement.Models;

namespace HRMedicalRecordsManagement.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordRepository _repository;

    public MedicalRecordService(IMedicalRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TMedicalRecord>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

     public async Task<TMedicalRecord> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
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
}