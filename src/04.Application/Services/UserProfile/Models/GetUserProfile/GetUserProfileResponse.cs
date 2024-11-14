namespace Pertamina.SIMIT.Application.Services.UserProfile.Models.GetUserProfile;

public class GetUserProfileResponse
{
    public Guid Id { get; set; }
    public string Username { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string EmployeeId { get; set; } = default!;
    public string CompanyCode { get; set; } = default!;
    public string EmailAddress { get; set; } = default!;
    public string MobilePhone { get; set; } = default!;
}
