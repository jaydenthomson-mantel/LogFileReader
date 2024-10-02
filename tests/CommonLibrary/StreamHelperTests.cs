
using CommonLibrary.Helpers;
using FluentAssertions;

namespace CommonLibraryTests;

public class StreamHelperTests
{
    [Fact]
    public void ReadEmbeddedResourceAsStream_ThrowsArgumentException_WhenResourceNotFound()
    {
        // Arrange
        var resourceName = "NonExistentResource.txt";
        
        // Act
        Action act = () => StreamHelpers.ReadEmbeddedResourceAsStream(resourceName);
        
        // Assert
        act.ShouldThrow<ArgumentException>()
            .WithMessage($"Resource '{resourceName}' not found in assembly.");
    }
}