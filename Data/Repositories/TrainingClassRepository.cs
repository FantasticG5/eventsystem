using Data.Contexts;
using Data.Entities;
using Data.Interfaces;

namespace Data.Repositories;

public class TrainingClassRepository(DataContext context)
    : BaseRepository<TrainingClassEntity>(context), ITrainingClassRepository
{

}
