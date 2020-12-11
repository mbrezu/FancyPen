using System;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FancyPen.Tests
{
    public class RenderTests
    {
        private string _nl = Environment.NewLine;

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

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"hello{_nl} {_nl}world!");
        }

        [Fact]
        public void AlwaysBreakWithIndent()
        {
            // Arrange
            var doc = Document.Indent(2, Document.AlwaysBreak(
                "hello".AsDocument(),
                "world!".AsDocument()
            ));
            var renderer = new Renderer();
            var sb = new StringBuilder();

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"  hello{_nl}  world!");
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

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"hello, {_nl}world!");
        }

        [Fact]
        public void MaybeBreakWithBreaksAndIndentation()
        {
            // Arrange
            var doc = Document.NeverBreak("    ".AsDocument(), Document.MaybeBreak(
                "hello, ".AsDocument(),
                "world!".AsDocument()
            ));
            var renderer = new Renderer(16);
            var sb = new StringBuilder();

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"    hello, {_nl}world!");
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

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"hello, world!");
        }

        [Fact]
        public void MildlyUseful()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "[".AsDocument(),
                Document.Indent(2,
                    Document.MaybeBreak(
                        "a, ".AsDocument(),
                        "b, ".AsDocument(),
                        "c".AsDocument())),
                "]".AsDocument());
            var renderer = new Renderer(2);
            var sb = new StringBuilder();

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"[{_nl}  a, {_nl}  b, {_nl}  c{_nl}]");
        }

        [Fact]
        public void MildlyUsefulTake2()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "[".AsDocument(),
                Document.Indent(1,
                    Document.MaybeBreak(
                        "a, ".AsDocument(),
                        "b, ".AsDocument(),
                        "c".AsDocument())),
                "]".AsDocument());
            var renderer = new Renderer(8);
            var sb = new StringBuilder();

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"[{_nl} a, b, c{_nl}]");
        }

        [Fact]
        public void WithSeparator()
        {
            // Arrange
            var doc = Document.NeverBreak(
                "[".AsDocument(),
                    Document.NeverBreak(
                        Document.WithSeparator(",".AsDocument(),
                            "a".AsDocument(),
                            "b".AsDocument(),
                            "c".AsDocument())),
                "]".AsDocument());
            var renderer = new Renderer();
            var sb = new StringBuilder();

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"[a,b,c]");
        }

        [Fact]
        public void SaveIndentation()
        {
            // Arrange
            var doc = Document.NeverBreak(
                "[".AsDocument(),
                    Document.SaveIndentation(
                        Document.MaybeBreak(
                            Document.WithSeparator(",".AsDocument(),
                                "a".AsDocument(),
                                "b".AsDocument(),
                                "c".AsDocument()))),
                "]".AsDocument());
            var renderer = new Renderer(5);
            var sb = new StringBuilder();

            // Act
            renderer.Render(doc, sb);

            // Assert
            sb.ToString().Should().Be($"[a,{_nl} b,{_nl} c]");
        }
    }
}
