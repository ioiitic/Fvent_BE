using Fvent.BO.Entities;
using Fvent.BO.Exceptions;
using Fvent.Repository.UOW;
using Fvent.Service.Mapper;
using Fvent.Service.Result;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using static Fvent.Service.Specifications.EventRegistationSpec;
using static Fvent.Service.Specifications.EventSpec;
using static Fvent.Service.Specifications.FormSpec;
using static Fvent.Service.Specifications.UserSpec;

namespace Fvent.Service.Services.Imp
{
    public class RegistationService(IUnitOfWork uOW) : IRegistationService
    {
        public async Task<IdRes> RegisterFreeEvent(Guid eventId, Guid userId)
        {
            var user = await uOW.Users.GetByIdAsync(userId);
            if(user.IsBanned)
            {
                throw new Exception("Your account has been banned");
            }
            var spec = new GetEventSpec(eventId);
            var events = await uOW.Events.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(Event));

            var regis = events!.Registrations!.Any(r => r.UserId == userId);
            if (regis) 
                throw new ValidationException("This event has been register");

            EventRegistration _eventRegis = new(eventId, userId);
            if (events!.MaxAttendees != null)
            {
                if (events.MaxAttendees == 0)
                {
                    throw new ValidationException("Sự kiện này đã hết chỗ trống!");
                }
                events.MaxAttendees = events.MaxAttendees - 1;
            }
            await uOW.EventRegistration.AddAsync(_eventRegis);
            await uOW.SaveChangesAsync();

            return _eventRegis.EventId.ToResponse();
        }

        public async Task UnRegisterEvent(Guid eventId, Guid userId)
        {
            var spec = new GetEventRegistrationSpec(eventId, userId);
            var regis = await uOW.EventRegistration.FindFirstOrDefaultAsync(spec)
                ?? throw new NotFoundException(typeof(EventRegistration));

            if (regis.IsCheckIn)
            {
                throw new Exception("Sau khi tham gia bạn không thể hủy đăng ký!");
            }

            uOW.EventRegistration.Delete(regis);

            var specEvent = new GetEventSpec(eventId);
            var events = await uOW.Events.FindFirstOrDefaultAsync(specEvent);

            var specSub = new GetFormSubmitSpec(eventId, userId);
            var formsubmit = await uOW.FormSubmit.FindFirstOrDefaultAsync(specSub);

            if (formsubmit != null)
            {
                uOW.FormSubmit.Delete(formsubmit);
            }

            if (events.MaxAttendees != null)
            {
                events.MaxAttendees = events.MaxAttendees + 1;
            }

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
