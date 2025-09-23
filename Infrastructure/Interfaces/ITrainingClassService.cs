using Infrastructure.Dtos;

namespace Infrastructure.Interfaces;

public interface ITrainingClassService
{
    Task<List<TrainingClassDto>> GetAllTrainingClassesAsync(CancellationToken ct = default);
    Task<TrainingClassDto?> GetTrainingClassByIdAsync(int id, CancellationToken ct = default);

    Task<bool> TryReserveSeatsAsync(int classId, int seats, CancellationToken ct = default);
    Task<bool> TryReleaseSeatsAsync(int classId, int seats, CancellationToken ct = default);
}