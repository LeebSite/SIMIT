namespace Pertamina.SIMIT.Domain.Interfaces;

public interface ICreatable
{
    DateTimeOffset Created { get; set; }
    string CreatedBy { get; set; }
}
