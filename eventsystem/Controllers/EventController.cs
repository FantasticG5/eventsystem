using Infrastructure.Interfaces;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eventsystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController(ITrainingClassService trainingClassService) : ControllerBase
{
    private readonly ITrainingClassService _trainingClassService = trainingClassService;

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        var trainingClass = await _trainingClassService.GetTrainingClassByIdAsync(id, ct);

        if (trainingClass == null)
            return NotFound();

        return Ok(trainingClass);
    }

}
