using Fvent.BO.Entities;
using Fvent.Repository.Common;
using Fvent.Repository.Data;

namespace Fvent.Repository.Repositories.Imp;

public class EventRegistrationRepo(MyDbContext context) : BaseRepository<EventRegistration>(context), IEventRegistrationRepo
{
}
