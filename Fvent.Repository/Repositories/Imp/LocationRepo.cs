using Fvent.BO.Entities;
using Fvent.Repository.Common;
using Fvent.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Repository.Repositories.Imp;
public class LocationRepo(MyDbContext context) : BaseRepository<Location>(context), ILocationRepo
{
}
