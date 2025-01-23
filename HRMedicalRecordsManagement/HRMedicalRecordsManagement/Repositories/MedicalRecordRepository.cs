using Microsoft.EntityFrameworkCore;
using HRMedicalRecordsManagement.Models;
using HRMedicalRecordsManagement.Data;

namespace HRMedicalRecordsManagement.Repositories;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TMedicalRecord>> GetAllAsync()
    {
        return await _context.TMedicalRecords
            .Include(r => r.Status) // Include related entities if necessary
            .Include(r => r.MedicalRecordType)
            .ToListAsync();
    }

    public async Task<TMedicalRecord> GetByIdAsync(int id)
    {
        var medicalRecord = await _context.TMedicalRecords
            .Include(r => r.Status) // Include related entities
            .Include(r => r.MedicalRecordType)
            .FirstOrDefaultAsync(r => r.MedicalRecordId == id);
        
        if(medicalRecord == null)
        {
            throw new KeyNotFoundException("Medical record not found");
        }

        return medicalRecord;

    }

    public async Task AddAsync(TMedicalRecord medicalRecord)
    {
        await _context.TMedicalRecords.AddAsync(medicalRecord);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TMedicalRecord medicalRecord)
    {
        _context.TMedicalRecords.Update(medicalRecord);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var record = await _context.TMedicalRecords.FindAsync(id);
        if (record != null)
        {
            _context.TMedicalRecords.Remove(record);
            await _context.SaveChangesAsync();
        }
    }
}