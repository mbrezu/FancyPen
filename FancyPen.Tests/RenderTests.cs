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
                "hello".AsDocument(),
                " ".AsDocument(),
                "world!".AsDocument()
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
                "hello".AsDocument(),
                " ".AsDocument(),
                "world!".AsDocument()
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
                "hello".AsDocument(),
                "world!".AsDocument()
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
                "hello, ".AsDocument(),
                "world!".AsDocument()
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
            var doc = Document.NeverBreak("    ".AsDocument(), Document.MaybeBreak(
                "hello, ".AsDocument(),
                "world!".AsDocument()
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
                "hello, ".AsDocument(),
                "world!".AsDocument()
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
                "[".AsDocument(),
                Document.Indent(2,
                    Document.MaybeBreak(
                        "a, ".AsDocument(),
                        "b, ".AsDocument(),
                        "c".AsDocument())),
                "]".AsDocument());
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
                "[".AsDocument(),
                Document.Indent(1,
                    Document.MaybeBreak(
                        "a, ".AsDocument(),
                        "b, ".AsDocument(),
                        "c".AsDocument())),
                "]".AsDocument());
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
                "[".AsDocument(),
                    Document.NeverBreak(
                        Document.WithSeparator(",".AsDocument(),
                            "a".AsDocument(),
                            "b".AsDocument(),
                            "c".AsDocument())),
                "]".AsDocument());
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
                "[".AsDocument(),
                    Document.SaveIndentation(
                        Document.MaybeBreak(
                            Document.WithSeparator(",".AsDocument(),
                                "a".AsDocument(),
                                "b".AsDocument(),
                                "c".AsDocument()))),
                "]".AsDocument());
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
                "hello, ".AsDocument(),
                "world!".AsDocument()
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
                "[".AsDocument(),
                Document.MaybeBreakSeparator(
                    " ".AsDocument(),
                    Document.WithSeparator(
                        ",".AsDocument(),
                        "a".AsDocument(),
                        "b".AsDocument(),
                        "c".AsDocument()
                    )
                ),
                "]".AsDocument());
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
                "a".AsDocument(),
                "b".AsDocument(),
                "c".AsDocument(),
                MakeArray(
                    "d".AsDocument(),
                    "e".AsDocument(),
                    "f".AsDocument())
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
                "[".AsDocument(),
                Document.SaveIndentation(
                    Document.MaybeBreakSeparator(
                        " ".AsDocument(),
                        Document.WithSeparator(
                            ",".AsDocument(),
                            documents
                        )
                    )),
                "]".AsDocument());
        }
    }
}
