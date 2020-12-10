using System.Collections.Generic;

namespace FancyPen
{
    public static class DocumentExtensions
    {
        public static Document AsDocument(this string str)
        {
            return new StringDocument(str);
        }
    }
}