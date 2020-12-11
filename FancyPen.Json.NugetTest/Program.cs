using System;
using System.Text.Json;

namespace FancyPen.Json.NugetTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var json = JsonDocument.Parse("{\"a\": 2}");

            var result = PrettyPrinter.Print(json);

            Console.WriteLine(result);
        }
    }
}
