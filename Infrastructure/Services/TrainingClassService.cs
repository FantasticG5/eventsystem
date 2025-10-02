using Data.Interfaces;
using Infrastructure.Dtos;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class TrainingClassService(ITrainingClassRepository trainingRepository) : ITrainingClassService
{
    private readonly ITrainingClassRepository _trainingRepository = trainingRepository;

    

    public async Task<TrainingClassDto?> GetTrainingClassByIdAsync(int id, CancellationToken ct = default)
    {
        var result = await _trainingRepository.GetByIdAsync(id, ct);

        if (!result.Succeeded || result.Result == null)
            return null;

        var dto = new TrainingClassDto
        {
            Id = result.Result.Id,
            Title = result.Result.Title,
            Description = result.Result.Description,
            StartTime = result.Result.StartTime,
            EndTime = result.Result.EndTime,
            Location = result.Result.Location,
            Instructor = result.Result.Instructor,
            Capacity = result.Result.Capacity,
            ReservedSeats = result.Result.ReservedSeats
        };

        return dto;
    }

    public async Task<List<TrainingClassDto>> GetAllTrainingClassesAsync(CancellationToken ct = default)
    {
        var result = await _trainingRepository.GetAllAsync(ct);

        if (!result.Succeeded || result.Result == null)
            return new List<TrainingClassDto>();

        return result.Result.Select(x => new TrainingClassDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            Location = x.Location,
            Instructor = x.Instructor,
            Capacity = x.Capacity,
            ReservedSeats = x.ReservedSeats

        }).ToList();
    }

    public async Task<bool> TryReserveSeatsAsync(int classId, int seats, CancellationToken ct = default)
        => await _trainingRepository.TryReserveSeatsAsync(classId, seats, ct);

    public async Task<bool> TryReleaseSeatsAsync(int classId, int seats, CancellationToken ct = default)
        => await _trainingRepository.TryReleaseSeatsAsync(classId, seats, ct);

}
