using Fvent.BO.Entities;
using Fvent.Repository.Common;
using Fvent.Repository.Data;

namespace Fvent.Repository.Repositories.Imp;

public class FormRepo(MyDbContext context) : BaseRepository<Form>(context), IFormRepo
{
}
