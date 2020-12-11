using System;
using System.Text;
using FluentAssertions;
using Xunit;

namespace FancyPen.Tests
{
    public class RenderTests
    {
        private string _nl = Environment.NewLine;
        private StringBuilder _sb = new();

        [Fact]
        public void NeverBreakBasic()
        {
            // Arrange
            var doc = Document.NeverBreak(
                "hello",
                " ",
                "world!"
            );
            var renderer = new Renderer();

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be("hello world!");
        }

        [Fact]
        public void AlwaysBreakBasic()
        {
            // Arrange
            var doc = Document.AlwaysBreak(
                "hello",
                " ",
                "world!"
            );
            var renderer = new Renderer();

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"hello{_nl} {_nl}world!");
        }

        [Fact]
        public void AlwaysBreakWithIndent()
        {
            // Arrange
            var doc = Document.Indent(2, Document.AlwaysBreak(
                "hello",
                "world!"
            ));
            var renderer = new Renderer();

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"  hello{_nl}  world!");
        }

        [Fact]
        public void MaybeBreakWithBreaks()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "hello, ",
                "world!"
            );
            var renderer = new Renderer(10);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"hello, {_nl}world!");
        }

        [Fact]
        public void MaybeBreakWithBreaksAndIndentation()
        {
            // Arrange
            var doc = Document.NeverBreak("    ", Document.MaybeBreak(
                "hello, ",
                "world!"
            ));
            var renderer = new Renderer(16);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"    hello, {_nl}world!");
        }

        [Fact]
        public void MaybeBreakWithoutBreaks()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "hello, ",
                "world!"
            );
            var renderer = new Renderer();

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"hello, world!");
        }

        [Fact]
        public void MildlyUseful()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "[",
                Document.Indent(2,
                    Document.MaybeBreak(
                        "a, ",
                        "b, ",
                        "c")),
                "]");
            var renderer = new Renderer(2);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"[{_nl}  a, {_nl}  b, {_nl}  c{_nl}]");
        }

        [Fact]
        public void MildlyUsefulTake2()
        {
            // Arrange
            var doc = Document.MaybeBreak(
                "[",
                Document.Indent(1,
                    Document.MaybeBreak(
                        "a, ",
                        "b, ",
                        "c")),
                "]");
            var renderer = new Renderer(8);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"[{_nl} a, b, c{_nl}]");
        }

        [Fact]
        public void WithSeparator()
        {
            // Arrange
            var doc = Document.NeverBreak(
                "[",
                    Document.NeverBreak(
                        Document.WithSeparator(",",
                            "a",
                            "b",
                            "c")),
                "]");
            var renderer = new Renderer();

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"[a,b,c]");
        }

        [Fact]
        public void SaveIndentation()
        {
            // Arrange
            var doc = Document.NeverBreak(
                "[",
                    Document.SaveIndentation(
                        Document.MaybeBreak(
                            Document.WithSeparator(",",
                                "a",
                                "b",
                                "c"))),
                "]");
            var renderer = new Renderer(5);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"[a,{_nl} b,{_nl} c]");
        }

        [Fact]
        public void MaxLength()
        {
            // Arrange
            var doc = Document.NeverBreak(
                "hello, ",
                "world!"
            );
            var renderer = new Renderer(5, 5);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Length.Should().BeLessThan("hello, world!".Length);
        }

        [Fact]
        public void MaybeBreakSeparator()
        {
            // Arrange
            var doc = Document.NeverBreak(
                "[",
                Document.MaybeBreakSeparator(
                    " ",
                    Document.WithSeparator(
                        ",",
                        "a",
                        "b",
                        "c"
                    )
                ),
                "]");
            var renderer = new Renderer();

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be("[a, b, c]");
        }

        [Fact]
        public void MildlyComplex()
        {
            // Arrange
            var doc = MakeArray(
                "a",
                "b",
                "c",
                MakeArray(
                    "d",
                    "e",
                    "f")
            );
            var renderer = new Renderer(5);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be($"[a,{_nl} b,{_nl} c,{_nl} [d,{_nl}  e,{_nl}  f]]");
        }

        private Document MakeArray(params Document[] documents)
        {
            return Document.NeverBreak(
                "[",
                Document.SaveIndentation(
                    Document.MaybeBreakSeparator(
                        " ",
                        Document.WithSeparator(
                            ",",
                            documents
                        )
                    )),
                "]");
        }
    }
}
