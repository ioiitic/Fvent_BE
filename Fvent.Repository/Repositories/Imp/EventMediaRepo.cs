using Fvent.BO.Entities;
using Fvent.Repository.Common;
using Fvent.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class EventMediaRepo(MyDbContext context) : BaseRepository<EventMedia>(context), IEventMediaRepo
{
}
