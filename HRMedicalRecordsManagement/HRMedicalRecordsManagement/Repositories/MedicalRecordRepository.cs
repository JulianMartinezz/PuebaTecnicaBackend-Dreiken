using Microsoft.EntityFrameworkCore;
using HRMedicalRecordsManagement.Models;
using HRMedicalRecordsManagement.Data;
using HRMedicalRecordsManagement.Common.PagedList;
using HRMedicalRecordsManagement.DTOs;
using AutoMapper;
using HRMedicalRecordsManagement.Common.DeletionData;

namespace HRMedicalRecordsManagement.Repositories;

public class MedicalRecordRepository : IMedicalRecordRepository
{
    private readonly ApplicationDbContext _context;

    public MedicalRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public void DetachEntity(TMedicalRecord entity)
    {
        if (_context.Entry(entity).State == EntityState.Detached)
            return;
        
        _context.Entry(entity).State = EntityState.Detached;
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

    public async Task DeleteAsync(int id, DeletionData deletionData)
    {
        var record = await _context.TMedicalRecords.FindAsync(id);
        if (record != null)
        {
            //Logical deletion
            record.StatusId = 2;
            record.DeletedBy = deletionData.CurrentUser;
            record.DeletionReason = deletionData.Reason;
            record.DeletionDate = deletionData.CurrentDate;
            _context.TMedicalRecords.Update(record);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<PagedList<TMedicalRecord>> GetFilteredMedicalRecordsAsync(
        int page,
        int pageSize,
        int? statusId,
        DateTime? startDate,
        DateTime? endDate,
        int? medicalRecordTypeId)
    {
        var query = _context.TMedicalRecords.AsQueryable();

        // Apply filters
        if (statusId.HasValue)
        {
            query = query.Where(r => r.StatusId == statusId.Value);
        }

        if (startDate.HasValue)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate.Value);
            query = query.Where(r => r.StartDate >= startDateOnly);
        }

        if (endDate.HasValue)
        {
            var endDateOnly = DateOnly.FromDateTime(endDate.Value);
            query = query.Where(r => r.EndDate <= endDateOnly);
        }

        if (medicalRecordTypeId.HasValue)
        {
            query = query.Where(r => r.MedicalRecordTypeId == medicalRecordTypeId.Value);
        }

        // Pagination logic
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<TMedicalRecord>(items, totalCount, page, pageSize);
    }
    
}