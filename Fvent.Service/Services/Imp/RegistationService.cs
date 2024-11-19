using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Result;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static Fvent.Service.Specifications.EventRegistationSpec;
using static Fvent.Service.Specifications.FormSpec;

namespace Fvent.Service.Services.Imp
{
    public class RegistationService(IUnitOfWork uOW) : IRegistationService
    {
        public async Task<IdRes> RegisterFreeEvent(Guid eventId, Guid userId)
        {
            EventRegistration _eventFollower = new EventRegistration(eventId, userId);

            await uOW.EventRegistration.AddAsync(_eventFollower);
            await uOW.SaveChangesAsync();

            return _eventFollower.EventId.ToResponse();
        }

        public async Task UnRegisterEvent(Guid eventId, Guid userId)
        {
            var spec = new GetEventRegistrationSpec(eventId, userId);
            var regis = await uOW.EventRegistration.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(User));

            uOW.EventRegistration.Delete(regis);

            var specForm = new GetFormSubmitSpec(eventId, userId);
            var res = await uOW.FormSubmit.FindFirstOrDefaultAsync(specForm)
                ?? throw new NotFoundException(typeof(FormSubmit));

            uOW.FormSubmit.Delete(res);
            await uOW.SaveChangesAsync();

            await uOW.SaveChangesAsync();
        }

        public async Task<IList<UserRes>> GetAllParticipantsForEvent(Guid eventId)
        {
            var spec = new GetEventParticipantsSpec(eventId);

            var participants = await uOW.EventRegistration.GetListAsync(spec);

            var participantResponses = participants
                .Select(p => p.User!.ToResponse<UserRes>())
                .ToList();

            return participantResponses;
        }

    }
}
