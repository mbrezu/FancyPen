using System;
using System.Text;

namespace FancyPen.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = Document.NeverBreak(
                "hello".AsDocument(),
                " ".AsDocument(),
                "world!".AsDocument()
            );
            var renderer = new Renderer();
            var sb = new StringBuilder();
            renderer.Render(doc, sb);
            Console.WriteLine(sb.ToString());
        }
    }
}
