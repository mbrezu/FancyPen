using System;
using System.Linq;
using System.Text;

namespace FancyPen
{
    public class Renderer
    {
        public void Render(Document document, StringBuilder destination)
        {
            switch (document)
            {
                case StringDocument d:
                    destination.Append(d.Content);
                    break;
                case NeverBreak d:
                    RenderNeverBreak(destination, d);
                    break;
                case AlwaysBreak d:
                    RenderAlwaysBreak(destination, d);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private void RenderNeverBreak(StringBuilder destination, NeverBreak d)
        {
            foreach (var child in d.Children)
            {
                Render(child, destination);
            }
        }

        private void RenderAlwaysBreak(StringBuilder destination, AlwaysBreak d)
        {
            if (d.Children.Any()) {
                Render(d.Children.First(), destination);
            }
            var rest = d.Children.Skip(1);
            if (rest.Any()) {
                foreach (var child in rest) {
                    destination.AppendLine();
                    Render(child, destination);
                }
            }
        }
    }
}
