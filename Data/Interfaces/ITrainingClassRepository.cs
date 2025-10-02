using Data.Entities;

namespace Data.Interfaces;

public interface ITrainingClassRepository : IBaseRepository<TrainingClassEntity>
{
    /// <summary>
    /// Försöker reservera ett antal platser för en klass.
    /// Returnerar false om klassen inte finns, om det blir överkapacitet
    /// eller om en concurrency-konflikt inträffar.
    /// </summary>
    Task<bool> TryReserveSeatsAsync(int classId, int seats, CancellationToken ct = default);

    /// <summary>
    /// Försöker släppa (minska) ett antal reserverade platser.
    /// Returnerar false om klassen inte finns eller vid concurrency-konflikt.
    /// Antalet reserverade platser understiger aldrig noll.
    /// </summary>
    Task<bool> TryReleaseSeatsAsync(int classId, int seats, CancellationToken ct = default);
}