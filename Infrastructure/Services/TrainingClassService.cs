using Data.Interfaces;
using Infrastructure.Dtos;
using Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class TrainingClassService(ITrainingClassRepository trainingRepository) : ITrainingClassService
{
    private readonly ITrainingClassRepository _trainingRepository = trainingRepository;

    

    public async Task<TrainingClassDto?> GetTrainingClassByIdAsync(int id, CancellationToken ct = default)
    {
        // Hämta entity från repository
        var result = await _trainingRepository.GetByIdAsync(id, ct);

        // Om anropet misslyckades eller resultatet är null
        if (!result.Succeeded || result.Result == null)
            return null;

        // Mappa entity till DTO
        var dto = new TrainingClassDto
        {
            Id = result.Result.Id,
            Title = result.Result.Title,
            Description = result.Result.Description,
            StartTime = result.Result.StartTime,
            EndTime = result.Result.EndTime,
            Location = result.Result.Location,
            Instructor = result.Result.Instructor,
            Capacity = result.Result.Capacity
        };

        return dto;
    }

    public async Task<List<TrainingClassDto>> GetAllTrainingClassesAsync(CancellationToken ct = default)
    {
        // Hämta entity från repository
        var result = await _trainingRepository.GetAllAsync(ct);

        // Om anropet misslyckades eller resultatet är null
        if (!result.Succeeded || result.Result == null)
            return new List<TrainingClassDto>();

        // Mappa entity till DTO
        return result.Result.Select(x => new TrainingClassDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            StartTime = x.StartTime,
            EndTime = x.EndTime,
            Location = x.Location,
            Instructor = x.Instructor,
            Capacity = x.Capacity
        }).ToList();
    }

}
