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
                case NeverBreak doc:
                    RenderNeverBreak(doc, destination);
                    break;
                case AlwaysBreak doc:
                    RenderAlwaysBreak(doc, destination);
                    break;
                case MaybeBreak doc:
                    RenderMaybeBreak(null, doc.Children, destination);
                    break;
                case MaybeBreakSeparator doc:
                    RenderMaybeBreak(doc.Separator, doc.Children, destination);
                    break;
                case Indent doc:
                    RenderIndent(destination, doc);
                    break;
                case SaveIndentation doc:
                    RenderSaveIndentation(destination, doc);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RenderNeverBreak(NeverBreak doc, StringBuilder destination)
        {
            foreach (var child in doc.Children)
            {
                RenderImpl(child, destination);
            }
        }

        private void RenderAlwaysBreak(AlwaysBreak doc, StringBuilder destination)
        {
            if (doc.Children.Any()) {
                RenderImpl(doc.Children.First(), destination);
            }
            var rest = doc.Children.Skip(1);
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

        private void RenderMaybeBreak(
            Document separator,
            IEnumerable<Document> children,
            StringBuilder destination)
        {
            var otherRenderer = new Renderer(int.MaxValue);
            otherRenderer.MaxLength = _maxColumn;
            var oneLineDestination = new StringBuilder();
            if (separator != null)
            {
                otherRenderer.Render(
                    new NeverBreak(Document.WithSeparator(separator, children.ToArray())),
                    oneLineDestination);
            }
            else
            {
                otherRenderer.Render(new NeverBreak(children), oneLineDestination);
            }
            if (_currentLine.Length + oneLineDestination.Length <= _maxColumn)
            {
                _currentLine.Append(oneLineDestination);
            }
            else
            {
                RenderImpl(new AlwaysBreak(children), destination);
            }
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

        private void RenderSaveIndentation(StringBuilder destination, SaveIndentation doc)
        {
            var oldIndentation = _indentation;
            _indentation = _currentLine.Length;
            RenderImpl(doc.Child, destination);
            _indentation = oldIndentation;
        }
    }
}
