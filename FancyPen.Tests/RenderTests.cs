using System;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FancyPen.Tests
{
    public class RenderTests
    {
        [Fact]
        public void NeverBreakBasicTest()
        {
            // Arrange
            var doc = Document.NoBreak(
                "hello".AsDocument(),
                " ".AsDocument(),
                "world!".AsDocument()
            );
            var renderer = new Renderer();
            var sb = new StringBuilder();

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be("hello world!");
        }


        [Fact]
        public void AlwaysBreakBasicTest()
        {
            // Arrange
            var doc = Document.AlwaysBreak(
                "hello".AsDocument(),
                " ".AsDocument(),
                "world!".AsDocument()
            );
            var renderer = new Renderer();
            var sb = new StringBuilder();
            var nl = Environment.NewLine;

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"hello{nl} {nl}world!");
        }
    }
}
