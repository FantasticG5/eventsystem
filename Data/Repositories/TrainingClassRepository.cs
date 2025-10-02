using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TrainingClassRepository(DataContext context)
    : BaseRepository<TrainingClassEntity>(context), ITrainingClassRepository
{
    public async Task<bool> TryReserveSeatsAsync(int classId, int seats, CancellationToken ct = default)
    {
        if (seats <= 0) return false;

        var entity = await _context.TrainingClasses
            .FirstOrDefaultAsync(x => x.Id == classId, ct);
        if (entity is null) return false;

        if (entity.ReservedSeats + seats > entity.Capacity) return false;

        entity.ReservedSeats += seats;

        try
        {
            await _context.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }

    public async Task<bool> TryReleaseSeatsAsync(int classId, int seats, CancellationToken ct = default)
    {
        if (seats <= 0) return false;

        var entity = await _context.TrainingClasses
            .FirstOrDefaultAsync(x => x.Id == classId, ct);
        if (entity is null) return false;

        entity.ReservedSeats = Math.Max(0, entity.ReservedSeats - seats);

        try
        {
            await _context.SaveChangesAsync(ct);
            return true;
        }
        catch (DbUpdateConcurrencyException)
        {
            return false;
        }
    }
}