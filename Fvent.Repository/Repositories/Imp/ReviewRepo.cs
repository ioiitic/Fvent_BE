using Fvent.BO.Entities;
using Fvent.Repository.Common;
using Fvent.Repository.Data;

namespace Fvent.Repository.Repositories.Imp;

public class ReviewRepo(MyDbContext context) : BaseRepository<EventReview>(context), IReviewRepo
{
}
