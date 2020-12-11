using System;
using System.Text.Json;

namespace FancyPen.Playground
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = JsonDocument.Parse("{\"a\": 2}");

            var result = FancyPen.Json.PrettyPrinter.Print(json);

            Console.WriteLine(result);
        }
    }
}
