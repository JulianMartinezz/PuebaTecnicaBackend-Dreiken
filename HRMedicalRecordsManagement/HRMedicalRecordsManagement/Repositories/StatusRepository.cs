using HRMedicalRecordsManagement.Data;
using Microsoft.EntityFrameworkCore;

public class StatusRepository : IStatusRepository
{
    private readonly ApplicationDbContext _context;

    public StatusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> StatusExistsAsync(int statusId)
    {
        return await _context.Statuses.AnyAsync(s => s.StatusId == statusId);
    }
}