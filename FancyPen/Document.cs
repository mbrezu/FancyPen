using System.Collections.Generic;

namespace FancyPen
{
    public record Document()
    {
        public static Document NeverBreak(params Document[] children)
        {
            return new NeverBreak(children);
        }

        public static Document AlwaysBreak(params Document[] children)
        {
            return new AlwaysBreak(children);
        }

        public static Document Nest(int amount, Document child)
        {
            return new Nest(amount, child);
        }

        public static Document MaybeBreak(params Document[] children)
        {
            return new MaybeBreak(children);
        }
    }

    record StringDocument(string Content): Document;

    record NeverBreak(IEnumerable<Document> Children) : Document;

    record AlwaysBreak(IEnumerable<Document> Children) : Document;

    record Nest(int Amount, Document Child) : Document;

    record MaybeBreak(IEnumerable<Document> Children) : Document;
}