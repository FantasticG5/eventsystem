using Microsoft.AspNetCore.Mvc;
using Data.Interfaces;

namespace eventsystem.Controllers;


[ApiController]
[Route("api/trainingclasses/{id:int}/seats")]
public class TrainingClassSeatsController(ITrainingClassRepository repo) : ControllerBase
{
    private readonly ITrainingClassRepository _repo = repo;
    public record SeatsRequest(int Seats = 1);

    [HttpPost("reserve")]
    public async Task<IActionResult> Reserve(int id, [FromBody] SeatsRequest req, CancellationToken ct)
    {
        var seats = Math.Max(1, req?.Seats ?? 1);
        if (!await _repo.ExistsAsync(x => x.Id == id, ct))
            return NotFound(new { message = "Klassen finns inte." });

        var ok = await _repo.TryReserveSeatsAsync(id, seats, ct);
        if (ok) return Ok();
        return Conflict(new { message = "Kunde inte reservera plats (fullt eller concurrency-konflikt)." });
    }

    [HttpPost("release")]
    public async Task<IActionResult> Release(int id, [FromBody] SeatsRequest req, CancellationToken ct)
    {
        var seats = Math.Max(1, req?.Seats ?? 1);
        if (!await _repo.ExistsAsync(x => x.Id == id, ct))
            return NotFound(new { message = "Klassen finns inte." });

        var ok = await _repo.TryReleaseSeatsAsync(id, seats, ct);
        if (ok) return Ok();
        return Conflict(new { message = "Kunde inte släppa plats (concurrency-konflikt)." });
    }
}