using Fvent.Repository.Data;
using Fvent.Service.Request;
using Fvent.Service.Services.Imp;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/forms")]
[ApiController]
public class FormController(MyDbContext myDbContext) : ControllerBase
{
}
