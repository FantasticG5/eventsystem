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

}
