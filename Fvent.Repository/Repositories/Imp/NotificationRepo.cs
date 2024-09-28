using Fvent.BO.Entities;
using Fvent.Repository.Common;
using Fvent.Repository.Data;

namespace Fvent.Repository.Repositories.Imp;

public class NotificationRepo(MyDbContext context) : BaseRepository<Notification>(context), INotificationRepo
{
}
