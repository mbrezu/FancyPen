using System.Collections.Generic;

namespace FancyPen
{
    public record Document()
    {
        public static Document Concat(params Document[] children) 
            => new Concat(children);

        public static Document ConcatLines(params Document[] children)
            => new ConcatLines(children);

        public static Document Indent(int amount, Document child)
            => new Indent(amount, child);

        public static Document Format(params Document[] children)
            => new Format(children);

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

        public static Document SaveIndentation(Document child)
            => new SaveIndentation(child);

        public static Document FormatSeparator(Document separator, params Document[] children)
            => new FormatSeparator(separator, children);

        public static implicit operator Document(string str) => new StringDocument(str);
    }

    record StringDocument(string Content): Document;

    record Concat(IEnumerable<Document> Children) : Document;

    record ConcatLines(IEnumerable<Document> Children) : Document;

    record Indent(int Amount, Document Child) : Document;

    record Format(IEnumerable<Document> Children) : Document;

    record SaveIndentation(Document Child) : Document;

    record FormatSeparator(Document Separator, IEnumerable<Document> Children): Document;
}