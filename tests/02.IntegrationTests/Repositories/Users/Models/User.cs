namespace Pertamina.SIMIT.IntegrationTests.Repositories.Users.Models;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string DisplayName { get; set; } = "Contoh Display Name";
    public string EmployeeId { get; set; } = "Contoh-Employee-Id";

    public IList<Persona> Personas { get; set; } = new List<Persona>();
}
