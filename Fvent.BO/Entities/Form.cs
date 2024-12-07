namespace Fvent.BO.Entities;

public class Form
{
    public Guid FormId { get; set; }
    public DateTime CreatedAt { get; set; }

    public IList<FormDetail>? FormDetails { get; set; }
}
