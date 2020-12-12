using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FancyPen
{
    public class Renderer
    {
        private int _indentation = 0;
        private int _maxColumn;
        private int MaxLength { get; set; } = int.MaxValue;
        private StringBuilder _currentLine;

        public Renderer(int maxColumn = 80)
        {
            _maxColumn = maxColumn;
        }

        public void Render(Document document, StringBuilder destination)
        {
            _currentLine = new StringBuilder();
            RenderImpl(document, destination);
            destination.Append(_currentLine);
        }

        public string Render(Document document)
        {
            var sb = new StringBuilder();
            Render(document, sb);
            return sb.ToString();
        }

        private void RenderImpl(Document document, StringBuilder destination)
        {
            if (destination.Length + _currentLine.Length > MaxLength)
            {
                return;
            }
            switch (document)
            {
                case StringDocument doc:
                    _currentLine.Append(doc.Content);
                    break;
                case Concat doc:
                    RenderConcat(doc.Children, destination);
                    break;
                case ConcatLines doc:
                    RenderConcatLines(doc.Children, destination);
                    break;
                case Format doc:
                    RenderFormat(null, doc.Children, destination);
                    break;
                case FormatSeparator doc:
                    RenderFormat(doc.Separator, doc.Children, destination);
                    break;
                case Indent doc:
                    RenderIndent(destination, doc);
                    break;
                case KeepIndentation doc:
                    RenderKeepIndentation(destination, doc);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RenderConcat(IEnumerable<Document> children, StringBuilder destination)
        {
            foreach (var child in children)
            {
                RenderImpl(child, destination);
            }
        }

        private void RenderConcatLines(IEnumerable<Document> children, StringBuilder destination)
        {
            if (children.Any()) {
                RenderImpl(children.First(), destination);
            }
            var rest = children.Skip(1);
            if (rest.Any()) {
                foreach (var child in rest) {
                    destination.Append(_currentLine);
                    _currentLine = new StringBuilder();
                    destination.AppendLine();
                    _currentLine.Append(new string(' ', _indentation));
                    RenderImpl(child, destination);
                }
            }
        }

        private void RenderFormat(
            Document separator,
            IEnumerable<Document> children,
            StringBuilder destination)
        {
            StringBuilder oneLine = RenderOnOneLine(separator, children);
            if (_currentLine.Length + oneLine.Length <= _maxColumn)
            {
                _currentLine.Append(oneLine);
            }
            else
            {
                RenderConcatLines(children, destination);
            }
        }

        private StringBuilder RenderOnOneLine(Document separator, IEnumerable<Document> children)
        {
            var otherRenderer = new Renderer(int.MaxValue);
            otherRenderer.MaxLength = _maxColumn;
            var oneLineDestination = new StringBuilder();
            if (separator != null)
            {
                var separatedChildren = Utils.WithSeparator(separator, children.ToArray());
                otherRenderer.Render(new Concat(separatedChildren), oneLineDestination);
            }
            else
            {
                otherRenderer.Render(new Concat(children), oneLineDestination);
            }

            return oneLineDestination;
        }

        private void RenderIndent(StringBuilder destination, Indent doc)
        {
            _indentation += doc.Amount;
            if (String.IsNullOrWhiteSpace(_currentLine.ToString()) && _currentLine.Length < _indentation)
            {
                _currentLine.Append(new string(' ', _indentation - _currentLine.Length));
            }
            RenderImpl(doc.Child, destination);
            _indentation -= doc.Amount;
        }

        private void RenderKeepIndentation(StringBuilder destination, KeepIndentation doc)
        {
            var oldIndentation = _indentation;
            _indentation = _currentLine.Length;
            RenderImpl(doc.Child, destination);
            _indentation = oldIndentation;
        }
    }
}
