using System;
using System.Text;

namespace FancyPen.NugetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var doc = Document.Concat(
                "hello",
                " ",
                "world!"
            );
            var renderer = new Renderer();
            var sb = new StringBuilder();

            renderer.Render(doc, sb);

            Console.WriteLine(sb);
        }
    }
}
