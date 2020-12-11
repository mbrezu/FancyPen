namespace FancyPen.Json
{
    public record PrettyPrinterOptions(IndentationOptions Indentation, int MaxColumn)
    {
        public static PrettyPrinterOptions Default 
            => new PrettyPrinterOptions(new KeepIndentationOption(), 80);
    }
}