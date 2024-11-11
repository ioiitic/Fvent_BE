using Fvent.Service.Request;
using Fvent.Service.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fvent.API.Controllers;

[Route("api/tags")]
[ApiController]
public class TagsController(ITagService TagService) : ControllerBase
{
    #region Tag
    /// <summary>
    /// Get list tags
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetList()
    {
        var res = await TagService.GetListTags();

        return Ok(res);
    }

    /// <summary>
    /// Get tag
    /// </summary>
    /// <param name="tagId"></param>
    /// <returns></returns>
    [HttpGet("{tagId}")]
    public async Task<IActionResult> GetTag(Guid tagId)
    {
        var res = await TagService.GetTag(tagId);

        return Ok(res);
    }

    /// <summary>
    /// Create a tag
    /// </summary>
    /// <param name="req"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateTag([FromBody] CreateTagReq req)
    {
        var res = await TagService.CreateTag(req.SvgContent, req.TagName);

        return Ok(res);
    }


    /// <summary>
    /// Delete a tag
    /// </summary>
    /// <param name="tagId"></param>
    /// <returns></returns>
    [HttpDelete("{tagId}")]
    public async Task<IActionResult> DeleteTag([FromRoute] Guid tagId)
    {
        await TagService.DeleteTag(tagId);

        return Ok();
    }
    #endregion
}
