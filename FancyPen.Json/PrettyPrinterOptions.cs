namespace FancyPen.Json
{
    public record Indentation();

    public record KeepIndentation() : Indentation;

    public record IndentAmount(int Amount): Indentation;

    public record PrettyPrinterOptions(Indentation Indentation, int MaxColumn)
    {
        public static PrettyPrinterOptions Default 
            => new PrettyPrinterOptions(new KeepIndentation(), 80);
    }
}