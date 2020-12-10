using System;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FancyPen.Tests
{
    public class RenderTests
    {
        [Fact]
        public void NeverBreakBasic()
        {
            // Arrange
            var doc = Document.NeverBreak(
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
        public void AlwaysBreakBasic()
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

        [Fact]
        public void AlwaysBreakWithNest()
        {
            // Arrange
            var doc = Document.Nest(2, Document.AlwaysBreak(
                "hello".AsDocument(),
                "world!".AsDocument()
            ));
            var renderer = new Renderer();
            var sb = new StringBuilder();
            var nl = Environment.NewLine;

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"hello{nl}  world!");
        }

        [Fact]
        public void MaybeBreakWithBreaks()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "hello, ".AsDocument(),
                "world!".AsDocument()
            );
            var renderer = new Renderer(10);
            var sb = new StringBuilder();
            var nl = Environment.NewLine;

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"hello, {nl}world!");
        }

        [Fact]
        public void MaybeBreakWithoutBreaks()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "hello, ".AsDocument(),
                "world!".AsDocument()
            );
            var renderer = new Renderer();
            var sb = new StringBuilder();
            var nl = Environment.NewLine;

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"hello, world!");
        }
    }
}
