using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace FancyPen.Json
{
    public static class PrettyPrinter
    {
        public static string Print(JsonDocument document)
        {
            return Print(document.RootElement);
        }

        public static string Print(JsonElement element)
        {
            var doc = PrintImpl(element);
            var renderer = new Renderer();
            var sb = new StringBuilder();
            renderer.Render(doc, sb);
            return sb.ToString();
        }

        private static Document PrintImpl(JsonElement element)
        {
            return element.ValueKind switch 
            {
                JsonValueKind.Array => PrintArray(element),
                JsonValueKind.String 
                    or JsonValueKind.Number
                    => JsonSerializer.Serialize(element),
                _ => throw new NotImplementedException()
            };
        }

        private static Document PrintArray(JsonElement element)
        {
            return Document.Concat(
                "[",
                Document.SaveIndentation(
                    Document.FormatSeparator(
                        " ",
                        Document.WithSeparator(
                            ",",
                            element.EnumerateArray().Select(element => PrintImpl(element)).ToArray()
                        )
                    )),
                "]");
        }
    }
}