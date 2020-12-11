using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace FancyPen.Json
{
    public static class PrettyPrinter
    {
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
            var sb = new StringBuilder();
            renderer.Render(doc, sb);
            return sb.ToString();
        }

        private static Document PrintImpl(JsonElement element, Indentation indentation)
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

        private static Document PrintArray(JsonElement element, Indentation indentation)
        {
            switch (indentation)
            {
                case KeepIndentation:
                    return PrintArrayKeepIndentation(element, indentation);
                case IndentAmount indent:
                    return PrintArrayIndentAmount(element, indent.Amount, indentation);
                default:
                    throw new NotImplementedException();
            }
        }

        private static Document PrintArrayKeepIndentation(
            JsonElement element,
            Indentation indentation)
        {
            return Document.Concat(
                "[ ",
                Document.SaveIndentation(
                    Document.FormatSeparator(
                        " ",
                        Document.WithSeparator(
                            ",",
                            element
                                .EnumerateArray()
                                .Select(element => PrintImpl(element, indentation)).ToArray()
                        )
                    )),
                " ]");
        }

        private static Document PrintArrayIndentAmount(
            JsonElement element,
            int amount,
            Indentation indentation)
        {
            return Document.Format(
                "[",
                Document.Indent(
                    amount,
                    Document.FormatSeparator(
                        " ",
                        Document.WithSeparator(
                            ",",
                            element
                                .EnumerateArray()
                                .Select(element => PrintImpl(element, indentation)).ToArray()
                        )
                    )),
                "]");
        }

        private static Document PrintObject(JsonElement element, Indentation indentation)
        {
            switch (indentation)
            {
                case KeepIndentation:
                    return PrintObjectKeepIndentation(element, indentation);
                case IndentAmount indent:
                    return PrintObjectIndentAmount(element, indent.Amount, indentation);
                default:
                    throw new NotImplementedException();
            }
        }

        private static Document PrintObjectKeepIndentation(JsonElement element, Indentation indentation)
        {
            return Document.Concat(
                "{ ",
                Document.SaveIndentation(
                    Document.FormatSeparator(
                        " ",
                        Document.WithSeparator(
                            ",",
                            element
                                .EnumerateObject()
                                .Select(element => PrintKeyValue(element, indentation)).ToArray()
                        )
                    )),
                " }");
        }

        private static Document PrintObjectIndentAmount(
            JsonElement element,
            int amount,
            Indentation indentation)
        {
            return Document.Format(
                "{",
                Document.Indent(
                    amount,
                    Document.FormatSeparator(
                        " ",
                        Document.WithSeparator(
                            ",",
                            element
                                .EnumerateObject()
                                .Select(element => PrintKeyValue(element, indentation)).ToArray()
                        )
                    )),
                "}");
        }

        private static Document PrintKeyValue(JsonProperty element, Indentation indentation)
        {
            return Document.Concat(
                JsonSerializer.Serialize(element.Name),
                ": ",
                PrintImpl(element.Value, indentation));
        }
    }
}