namespace Shared.Entities.Models.Audits;

public class Audit
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Type { get; set; }
    public string TableName { get; set; }
    public DateTime DateTime { get; set; }
    public string OldValues { get; set; }
    public string NewValues { get; set; }
    public string AffectedColumns { get; set; }
}