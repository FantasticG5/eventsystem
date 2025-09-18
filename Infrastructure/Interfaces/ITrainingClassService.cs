using Infrastructure.Dtos;

namespace Infrastructure.Interfaces
{
    public interface ITrainingClassService
    {
        Task<TrainingClassDto?> GetTrainingClassByIdAsync(int id, CancellationToken ct = default);
    }
}