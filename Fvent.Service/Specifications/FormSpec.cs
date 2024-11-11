using Fvent.BO.Entities;
using Fvent.Repository.Common;
using Microsoft.Extensions.Logging;

namespace Fvent.Service.Specifications;

public static class FormSpec
{
    public class GetFormSubmitSpec : Specification<FormSubmit>
    {
        public GetFormSubmitSpec(Guid eventId)
        {
            Filter(f => f.EventId == eventId);
        }
    }
}
