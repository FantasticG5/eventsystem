using Infrastructure.Dtos;

namespace Infrastructure.Interfaces
{
    public interface ITrainingClassService
    {
        Task<List<TrainingClassDto>> GetAllTrainingClassesAsync(CancellationToken ct = default);
        Task<TrainingClassDto?> GetTrainingClassByIdAsync(int id, CancellationToken ct = default);
    }
}