using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers
{
    [Route("api/tags")]
    [ApiController]
    public class TagsController(ITagService TagService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetList()
        {
            var res = await TagService.GetListTags();

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTag(Guid id)
        {
            var res = await TagService.GetTag(id);

            return Ok(res);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTag([FromBody] CreateTagReq req)
        {
            var res = await TagService.CreateTag(req.SvgContent, req.TagName);

            return Ok(res);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag([FromRoute] Guid id)
        {
            await TagService.DeleteTag(id);

            return Ok();
        }
    }
}
