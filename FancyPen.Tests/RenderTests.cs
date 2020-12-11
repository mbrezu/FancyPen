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
        public void ConcatBasic()
        {
            // Arrange
            var doc = Document.Concat(
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
        public void ConcatLinesBasic()
        {
            // Arrange
            var doc = Document.ConcatLines(
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
        public void ConcatLinesWithIndent()
        {
            // Arrange
            var doc = Document.Indent(2, Document.ConcatLines(
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
        public void FormatWithBreaks()
        {
            // Arrange
            var doc = Document.Format(
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
        public void FormatWithBreaksAndIndentation()
        {
            // Arrange
            var doc = Document.Concat("    ", Document.Format(
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
        public void FormatWithoutBreaks()
        {
            // Arrange
            var doc = Document.Format(
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
            var doc = Document.Format(
                "[",
                Document.Indent(2,
                    Document.Format(
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
            var doc = Document.Format(
                "[",
                Document.Indent(1,
                    Document.Format(
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
            var doc = Document.Concat(
                "[",
                    Document.Concat(
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
            var doc = Document.Concat(
                "[",
                    Document.SaveIndentation(
                        Document.Format(
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
        public void FormatSeparator()
        {
            // Arrange
            var doc = Document.Concat(
                "[",
                Document.FormatSeparator(
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
        public void MildlyComplexSaveIndentation()
        {
            // Arrange
            var doc = MakeArraySaveIndentation(
                "a",
                "b",
                "c",
                MakeArraySaveIndentation(
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

        [Fact]
        public void MildlyComplexIndent()
        {
            // Arrange
            var doc = MakeArrayIndent(
                "a",
                "b",
                "c",
                MakeArrayIndent(
                    "d",
                    "e",
                    "f")
            );
            var renderer = new Renderer(5);

            // Act
            renderer.Render(doc, _sb);

            // Assert
            _sb.ToString().Should().Be(@"[
    a,
    b,
    c,
    [
        d,
        e,
        f
    ]
]");
        }

        private Document MakeArraySaveIndentation(params Document[] documents)
        {
            return Document.Concat(
                "[",
                Document.SaveIndentation(
                    Document.FormatSeparator(
                        " ",
                        Document.WithSeparator(
                            ",",
                            documents
                        )
                    )),
                "]");
        }

        private Document MakeArrayIndent(params Document[] documents)
        {
            return Document.Format(
                "[",
                Document.Indent(
                    4,
                    Document.FormatSeparator(
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
