using System.ComponentModel;
using FluentAssertions;
using Pertamina.SIMIT.Shared.Common.Extensions;
using Xunit;

namespace Pertamina.SIMIT.UnitTests.Shared.Common.Extensions;

public class DescriptionAttributeExtensionsTests
{
    public const string SampleClassDescription = "This is my class";
    public const string SampleConstantDescription = "This is my constant";
    public const string SamplePropertyDescription = "This is my property";

    [Fact]
    public void Should_Get_Class_Description()
    {
        // Arrange
        var sampleObject = new SampleClass();

        // Act
        var classDescription = sampleObject.GetTypeDescription();

        // Assert
        classDescription.Should().Be(SampleClassDescription);
    }

    [Fact]
    public void Should_Get_Const_Field_Description()
    {
        // Act
        var constantDescription = DescriptionAttributeExtensions.GetConstFieldDescription<SampleClass>(nameof(SampleClass.SampleConstant));

        // Assert
        constantDescription.Should().Be(SampleConstantDescription);
    }

    [Fact]
    public void Should_Get_Member_Description()
    {
        // Arrange
        var sampleObject = new SampleClass();

        // Act
        var propertyDescription = sampleObject.GetDescription(nameof(SampleClass.SampleProperty));

        // Assert
        propertyDescription.Should().Be(SamplePropertyDescription);
    }
}

[Description(DescriptionAttributeExtensionsTests.SampleClassDescription)]
public class SampleClass
{
    [Description(DescriptionAttributeExtensionsTests.SampleConstantDescription)]
    public const string SampleConstant = "SampleConstantValue";

    [Description(DescriptionAttributeExtensionsTests.SamplePropertyDescription)]
    public int SampleProperty { get; set; }
}
