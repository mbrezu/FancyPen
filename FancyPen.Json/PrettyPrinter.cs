using System;
using System.Linq;
using System.Text.Json;

namespace FancyPen.Json
{
    public static class PrettyPrinter
    {
        private record ListConfig(BracedListConfig KeepIndentation, BracedListConfig IndentAmount);

        private static ListConfig _arrayConfig = new ListConfig(
            new BracedListConfig("[ ", " ]"), new BracedListConfig("[", "]"));
        private static ListConfig _objectConfig = new ListConfig(
            new BracedListConfig("{ ", " }"), new BracedListConfig("{", "}"));

        public static string Print(JsonDocument document, PrettyPrinterOptions options = null)
        {
            return Print(document.RootElement, options);
        }

        public static string Print(JsonElement element, PrettyPrinterOptions options = null)
        {
            if (options == null)
            {
                options = PrettyPrinterOptions.Default;
            }
            var doc = PrintImpl(element, options.Indentation);
            var renderer = new Renderer(options.MaxColumn);
            return renderer.Render(doc);
        }

        private static Document PrintImpl(JsonElement element, IndentationOptions indentation)
        {
            return element.ValueKind switch 
            {
                JsonValueKind.Array => PrintArray(element, indentation),
                JsonValueKind.Object => PrintObject(element, indentation),
                JsonValueKind.String 
                    or JsonValueKind.Number
                    or JsonValueKind.True
                    or JsonValueKind.False
                    or JsonValueKind.Null
                    => JsonSerializer.Serialize(element),
                _ => throw new NotImplementedException()
            };
        }

        private static Document PrintArray(JsonElement element, IndentationOptions indentation)
        {
            var children = element
                .EnumerateArray()
                .Select(element => PrintImpl(element, indentation))
                .ToArray();
            return CreateBracedList(_arrayConfig, indentation, children);
        }

        private static Document CreateBracedList(
            ListConfig listConfig,
            IndentationOptions indentation,
            Document[] children)
        {
            switch (indentation)
            {
                case KeepIndentationOption:
                    return Utils.BracedListKeepIndentation(listConfig.KeepIndentation, children);
                case IndentAmountOption indent:
                    return Utils.BracedListIndentAmount(listConfig.IndentAmount, indent.Amount, children);
                default:
                    throw new NotImplementedException();
            }
        }

        private static Document PrintObject(JsonElement element, IndentationOptions indentation)
        {
            var children = element
                .EnumerateObject()
                .Select(element => PrintKeyValue(element, indentation))
                .ToArray();
            return CreateBracedList(_objectConfig, indentation, children);
        }

        private static Document PrintKeyValue(JsonProperty element, IndentationOptions indentation)
        {
            return Document.Concat(
                JsonSerializer.Serialize(element.Name),
                ": ",
                PrintImpl(element.Value, indentation));
        }
    }
}