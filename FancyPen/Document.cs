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

        public static Document KeepIndentation(Document child)
            => new KeepIndentation(child);

        public static Document FormatSeparator(Document separator, params Document[] children)
            => new FormatSeparator(separator, children);

        public static implicit operator Document(string str) => new StringDocument(str);
    }

    record StringDocument(string Content): Document;

    record Concat(IEnumerable<Document> Children) : Document;

    record ConcatLines(IEnumerable<Document> Children) : Document;

    record Indent(int Amount, Document Child) : Document;

    record Format(IEnumerable<Document> Children) : Document;

    record KeepIndentation(Document Child) : Document;

    record FormatSeparator(Document Separator, IEnumerable<Document> Children): Document;
}