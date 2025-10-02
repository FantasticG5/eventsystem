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

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var classes = await _trainingClassService.GetAllTrainingClassesAsync(ct);
        if (classes == null)
            return NotFound();

        return Ok(classes);
    }

}
