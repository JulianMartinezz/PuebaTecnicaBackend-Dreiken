using HRMedicalRecordsManagement.Data;
using Microsoft.EntityFrameworkCore;

public class MedicalRecordTypeRepository : IMedicalRecordTypeRepository
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> MedicalRecordTypeExistsAsync(int medicalRecordTypeId)
    {
        return await _context.MedicalRecordTypes.AnyAsync(m => m.MedicalRecordTypeId == medicalRecordTypeId);
    }
}