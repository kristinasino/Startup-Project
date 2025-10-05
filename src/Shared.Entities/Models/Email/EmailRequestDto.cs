namespace Shared.Entities.Models.Email;

public class EmailRequestDto
{
    public string Subject { get; set; }
    public string Message { get; set; }
    public string[] Recipents { get; set; }
}