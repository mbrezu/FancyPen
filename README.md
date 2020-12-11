# Fancy Pen

> "All problems in computer science can be solved by another level of indirection."
> 
> David Wheeler

A library to help writers of pretty printers by providing a useful indirection. Heavily inspired by
Philip Wadler's ["A prettier printer"](http://homepages.inf.ed.ac.uk/wadler/papers/prettier/prettier.pdf) article.

Nuget package: https://www.nuget.org/packages/FancyPen/.

# Description

The indirection is a [Document](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Document.cs) which the [Renderer](https://github.com/mbrezu/FancyPen/blob/master/FancyPen/Renderer.cs) knows how to [Render](https://github.com/mbrezu/FancyPen/blob/master/FancyPen/Renderer.cs#L20).

To write a pretty printer, one needs to write code to create the Document from the
object that needs pretty printing (see [an example](https://github.com/mbrezu/FancyPen/blob/master/FancyPen.Json/PrettyPrinter.cs)).

A document is either:

 * a [string](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Document.cs#L28),
 * a [concatenation](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Document.cs#L30) of other documents,
 * a [new-line separated concatenation](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Document.cs#L32) of other documents,
 * a [formatted concatenation](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Document.cs#L36) of other documents (which is either a plain concatenation or one with new-lines based on what fits on the current line given the [max column](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Renderer.cs#L15)),
 * a [wrapper](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Document.cs#L34) that indents the wrapped document or
 * a [wrapper](https://github.com/mbrezu/FancyPen/blob/8bf9a226c32a32112101f88fed373603c8c0954d/FancyPen/Document.cs#L38) that keeps the current indentation for the wrapped document if new lines are added.

As examples of how to use these primitives, see the [Utils](https://github.com/mbrezu/FancyPen/blob/master/FancyPen/Utils.cs).

# Usage

See the [Renderer tests](https://github.com/mbrezu/FancyPen/blob/master/FancyPen.Tests/RenderTests.cs).

# Examples

As an example, a [JSON pretty printer](https://github.com/mbrezu/FancyPen/blob/master/FancyPen.Json/PrettyPrinter.cs) (for [JsonDocument](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsondocument?view=net-5.0) and [JsonElement](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonelement)) made with FancyPen is provided.

Sample formatted JSON:

```json
{
    "age": 20,
    "name": "John",
    "address": {
        "street": "Main", "number": 20, "city": "Smallville"
    },
    "array": [
        1000,
        2000,
        3000,
        4000,
        5000,
        6000,
        7000,
        8000,
        9000,
        10000
    ]
}
```

(notice how the short `address` content is on one line, but the `array` is split over multiple lines).

# License

[2-clause BSD](https://en.wikipedia.org/wiki/BSD_licenses#2-clause_license_.28.22Simplified_BSD_License.22_or_.22FreeBSD_License.22.29), see the [LICENSE](./LICENSE) file.