namespace Fvent.BO.Entities;

public class FormDetail
{
    public Guid FormDetailId { get; set; }

    public string Name { get; set; }
    public string Type { get; set; }
    public IList<string> Options { get; set; }
    public Guid FormId { get; set; }

    public Form? Form { get; set; }

    public FormDetail(string name, string type, IList<string> options)
    {
        Name = name;
        Type = type;
        Options = options;
    }
}
