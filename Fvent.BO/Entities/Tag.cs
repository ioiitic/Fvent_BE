namespace Fvent.BO.Entities;

public class Tag
{
    public Guid TagId { get; set; }
    public string SvgContent { get; set; }
    public string TagName { get; set; }
    public DateTime CreatedAt { get; set; }

    public Tag(string svgContent, string tagName)
    {
        SvgContent=svgContent;
        TagName=tagName;
        CreatedAt=DateTime.UtcNow;
    }
}
