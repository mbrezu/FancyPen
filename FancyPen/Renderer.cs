using System;
using System.Linq;
using System.Text;

namespace FancyPen
{
    public class Renderer
    {
        private int _indentation = 0;
        private int _maxColumn;

        public Renderer(int maxColumn = 80)
        {
            _maxColumn = maxColumn;
        }

        public void Render(Document document, StringBuilder destination)
        {
            switch (document)
            {
                case StringDocument doc:
                    destination.Append(doc.Content);
                    break;
                case NeverBreak doc:
                    RenderNeverBreak(destination, doc);
                    break;
                case AlwaysBreak doc:
                    RenderAlwaysBreak(destination, doc);
                    break;
                case Nest doc:
                    _indentation += doc.Amount;
                    Render(doc.Child, destination);
                    _indentation -= doc.Amount;
                    break;
                case MaybeBreak doc:
                    RenderMaybeBreak(doc, destination);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RenderNeverBreak(StringBuilder destination, NeverBreak doc)
        {
            foreach (var child in doc.Children)
            {
                Render(child, destination);
            }
        }

        private void RenderAlwaysBreak(StringBuilder destination, AlwaysBreak doc)
        {
            if (doc.Children.Any()) {
                Render(doc.Children.First(), destination);
            }
            var rest = doc.Children.Skip(1);
            if (rest.Any()) {
                foreach (var child in rest) {
                    destination.AppendLine();
                    destination.Append(new string(' ', _indentation));
                    Render(child, destination);
                }
            }
        }

        private void RenderMaybeBreak(MaybeBreak doc, StringBuilder destination)
        {
            var otherRenderer = new Renderer(int.MaxValue);
            var oneLineDestination = new StringBuilder();
            otherRenderer.Render(new NeverBreak(doc.Children), oneLineDestination);
            // FIXME: must consider length line so far (before the content in otherRender).
            if (oneLineDestination.Length < _maxColumn)
            {
                destination.Append(oneLineDestination);
            }
            else
            {
                Render(new AlwaysBreak(doc.Children), destination);
            }
        }
    }
}
