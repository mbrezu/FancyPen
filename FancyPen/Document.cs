using System.Collections.Generic;

namespace FancyPen
{
    public record Document()
    {
        public static Document NeverBreak(params Document[] children) 
            => new NeverBreak(children);

        public static Document AlwaysBreak(params Document[] children)
            => new AlwaysBreak(children);

        public static Document Indent(int amount, Document child)
            => new Indent(amount, child);

        public static Document MaybeBreak(params Document[] children)
            => new MaybeBreak(children);

        public static Document[] WithSeparator(Document separator, params Document[] children)
        {
            List<Document> result = new();
            for (int i = 0; i < children.Length; i++)
            {
                if (i < children.Length - 1)
                {
                    result.Add(Document.NeverBreak(children[i], separator));
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

        public static Document MaybeBreakSeparator(Document separator, params Document[] children)
            => new MaybeBreakSeparator(separator, children);

        public static implicit operator Document(string str) => new StringDocument(str);
    }

    record StringDocument(string Content): Document;

    record NeverBreak(IEnumerable<Document> Children) : Document;

    record AlwaysBreak(IEnumerable<Document> Children) : Document;

    record Indent(int Amount, Document Child) : Document;

    record MaybeBreak(IEnumerable<Document> Children) : Document;

    record SaveIndentation(Document Child) : Document;

    record MaybeBreakSeparator(Document Separator, IEnumerable<Document> Children): Document;
}