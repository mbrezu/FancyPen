using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace FancyPen.Tests
{
    public class JsonTests
    {
        [Fact]
        public void BasicArray()
        {
            // Arrange
            var json = JsonDocument.Parse("[\"a\", 2]");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(json);

            // Assert
            result.Should().Be("[\"a\", 2]");
        }

        [Fact]
        public void BasicObject()
        {
            // Arrange
            var json = JsonDocument.Parse("{\"a\": 2}");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(json);

            // Assert
            result.Should().Be("[\"a\", 2]");
        }
    }
}