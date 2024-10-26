using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Fvent.Service.Specifications.TagSpec;

namespace Fvent.Service.Services.Imp
{
    public class TagService(IUnitOfWork uOW) : ITagService
    {
        public async Task<IdRes> CreateTag(string SvgContent, string TagName)
        {
            var newTag = new Tag(SvgContent, TagName);

            await uOW.Tag.AddAsync(newTag);
            await uOW.SaveChangesAsync();

            return newTag.TagId.ToResponse();
        }

        public async Task DeleteTag(Guid id)
        {
            var spec = new GetTagSpec(id);
            var Tag = await uOW.Tag.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(EventReview));

            uOW.Tag.Delete(Tag);

            await uOW.SaveChangesAsync();
        }

        public async Task<IList<Tag>> GetListTags()
        {
            var spec = new GetTagSpec();
            var Tags = await uOW.Tag.GetListAsync(spec);

            return Tags.ToList();
        }

        public async Task<Tag> GetTag(Guid id)
        {
            var spec = new GetTagSpec(id);
            var Tag = await uOW.Tag.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(Tag));

            return Tag;
        }
    }
}
