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
    }

    public record StringDocument(string Content): Document;

    public record NeverBreak(IEnumerable<Document> Children) : Document;

    public record AlwaysBreak(IEnumerable<Document> Children) : Document;
}