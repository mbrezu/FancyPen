using System;
using System.Collections.Generic;

namespace FancyPen
{
    public static class Utils
    {
        public static Document[] WithSeparator(Document separator, params Document[] children)
        {
            List<Document> result = new();
            for (int i = 0; i < children.Length; i++)
            {
                if (i < children.Length - 1)
                {
                    result.Add(Document.Concat(children[i], separator));
                }
                else
                {
                    result.Add(children[i]);
                }
            }
            return result.ToArray();
        }

        public static Document BracedListKeepIndentation(
            BracedListConfig config,
            Document[] elements)
        {
            return Document.Concat(
                config.StartBrace,
                Document.KeepIndentation(
                    Document.FormatSeparator(
                        config.NewLineReplacement,
                        Utils.WithSeparator(config.ListSeparator, elements)
                    )),
                config.EndBrace);
        }

        public static Document BracedListIndentAmount(
            BracedListConfig config,
            int amount,
            Document[] elements)
        {
            return Document.Format(
                config.StartBrace,
                Document.Indent(
                    amount,
                    Document.FormatSeparator(
                        config.NewLineReplacement,
                        Utils.WithSeparator(config.ListSeparator, elements)
                    )),
                config.EndBrace);
        }
    }

    public record BracedListConfig(
        string StartBrace,
        string EndBrace,
        string ListSeparator = ",",
        string NewLineReplacement = " "
    );
}