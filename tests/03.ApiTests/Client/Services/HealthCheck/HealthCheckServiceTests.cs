using FluentAssertions;
using Pertamina.SIMIT.Shared.Services.HealthCheck.Constants;
using Xunit;

namespace Pertamina.SIMIT.ApiTests.Client.Services.HealthCheck;

public class HealthCheckServiceTests : IClassFixture<ClientFixture>
{
    private readonly ClientFixture _fixture;

    public HealthCheckServiceTests(ClientFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Should_Get_HealthCheck_Result()
    {
        var response = await _fixture.HealthCheckService.GetHealthCheckAsync(_fixture.BackEndOptions.HealthCheckApiUrl);

        response.Status.Should().Be(HealthCheckStatus.Healthy);

        foreach (var healthCheckEntry in response.Entries.Values)
        {
            healthCheckEntry.Status.Should().Be(HealthCheckStatus.Healthy);
        }
    }
}
