using Fvent.BO.Entities;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fvent.Service.Services
{
    public interface ITagService
    {
        Task<IdRes> CreateTag(string SvgContent, string TagName);
        Task DeleteTag(Guid id);
        Task<IList<Tag>> GetListTags();
        Task<Tag> GetTag(Guid id);
    }
}
