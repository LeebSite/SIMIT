using FluentAssertions;
using Pertamina.SIMIT.Shared.Common.Extensions;
using Xunit;

namespace Pertamina.SIMIT.UnitTests.Shared.Common.Extensions;

public class StringExtensionsTests
{
    [Theory]
    [InlineData("NegaraKesatuanRepublikIndonesia", "Negara Kesatuan Republik Indonesia")]
    [InlineData("Negara17Kesatuan08Republik1945Indonesia", "Negara 17 Kesatuan 08 Republik 1945 Indonesia")]
    [InlineData("Indonesia123", "Indonesia 123")]
    [InlineData("Indonesia 123", "Indonesia 123")]
    public void Should_Split_Words(string sentence, string expectedSentence)
    {
        // Act
        var splittedWords = sentence.SplitWords();

        // Assert
        splittedWords.Should().Be(expectedSentence);
    }
}
