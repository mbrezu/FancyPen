using System.Collections.Generic;

namespace FancyPen
{
    public record Document()
    {
        public static Document NoBreak(params Document[] children)
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
    }

    record StringDocument(string Content): Document;

    record NeverBreak(IEnumerable<Document> Children) : Document;

    record AlwaysBreak(IEnumerable<Document> Children) : Document;

    record Nest(int Amount, Document Child) : Document;
}