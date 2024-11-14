using Pertamina.SIMIT.IntegrationTests.Application;
using Xunit;

namespace Pertamina.SIMIT.IntegrationTests;

[CollectionDefinition(nameof(ApplicationFixture), DisableParallelization = true)]
public class LaunchSettingsCollection : ICollectionFixture<ApplicationFixture>
{
}
