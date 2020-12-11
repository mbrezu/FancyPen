namespace FancyPen
{
    public record IndentationOptions();

    public record KeepIndentationOption() : IndentationOptions;

    public record IndentAmountOption(int Amount): IndentationOptions;
}