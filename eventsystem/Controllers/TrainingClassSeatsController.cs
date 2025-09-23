using Microsoft.AspNetCore.Mvc;
using Data.Interfaces;
using Infrastructure.Interfaces;

namespace eventsystem.Controllers;


[ApiController]
[Route("api/trainingclasses/{id:int}/seats")]
public class TrainingClassSeatsController(ITrainingClassService service) : ControllerBase
{
    private readonly ITrainingClassService _service = service;
    public record SeatsRequest(int Seats = 1);

    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve(int id, [FromBody] SeatsRequest req, CancellationToken ct)
    {
        var seats = Math.Max(1, req?.Seats ?? 1);
        if (await _service.GetTrainingClassByIdAsync(id, ct) == null)
            return NotFound(new { message = "Klassen finns inte." });

        var ok = await _service.TryReserveSeatsAsync(id, seats, ct);
        return ok
            ? Ok()
            : Conflict(new { message = "Kunde inte reservera plats (fullt eller concurrency-konflikt)." });
    }

    [HttpPost("release")]
    public async Task<IActionResult> Release(int id, [FromBody] SeatsRequest req, CancellationToken ct)
    {
        var seats = Math.Max(1, req?.Seats ?? 1);
        if (await _service.GetTrainingClassByIdAsync(id, ct) == null)
            return NotFound(new { message = "Klassen finns inte." });

        var ok = await _service.TryReleaseSeatsAsync(id, seats, ct);
        return ok
            ? Ok()
            : Conflict(new { message = "Kunde inte släppa plats (concurrency-konflikt)." });
    }
}