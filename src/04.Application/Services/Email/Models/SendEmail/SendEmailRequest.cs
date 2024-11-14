namespace Pertamina.SIMIT.Application.Services.Email.Models.SendEmail;

public class SendEmailRequest
{
    public IList<string> Tos { get; set; } = new List<string>();
    public IList<string> Ccs { get; set; } = new List<string>();
    public IList<string> Bccs { get; set; } = new List<string>();
    public string Subject { get; set; } = default!;
    public string Body { get; set; } = default!;
}
