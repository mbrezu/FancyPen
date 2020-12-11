using System;
using System.Dynamic;
using System.Text.Json;
using FancyPen.Json;
using FluentAssertions;
using Xunit;

namespace FancyPen.Tests
{
    public class JsonTests
    {
        [Fact]
        public void BasicArray()
        {
            // Arrange
            var json = JsonDocument.Parse("[\"a\", 2]");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(json);

            // Assert
            result.Should().Be("[ \"a\", 2 ]");
        }

        [Fact]
        public void BasicObject()
        {
            // Arrange
            var json = JsonDocument.Parse("{\"a\": 2}");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(json);

            // Assert
            result.Should().Be("{ \"a\": 2 }");
        }

        [Fact]
        public void Primitives()
        {
            // Arrange
            var json = JsonDocument.Parse("[\"a\", 2, true, false, null]");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(json);

            // Assert
            result.Should().Be("[ \"a\", 2, true, false, null ]");
        }

        [Fact]
        public void ArrayKeepIndentation()
        {
            // Arrange
            var json = JsonDocument.Parse("[100, 200, 300, 400, 500]");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(
                json,
                new PrettyPrinterOptions(new KeepIndentationOption(), 10));

            // Assert
            result.Should().Be(@"[ 100,
  200,
  300,
  400,
  500 ]".Replace("\n", Environment.NewLine));
        }

        [Fact]
        public void ArrayIndentAmount()
        {
            // Arrange
            var json = JsonDocument.Parse("[100, 200, 300, 400, 500]");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(
                json,
                new PrettyPrinterOptions(new IndentAmountOption(4), 10));

            // Assert
            result.Should().Be(@"[
    100,
    200,
    300,
    400,
    500
]".Replace("\n", Environment.NewLine));
        }

        [Fact]
        public void ObjectKeepIndentation()
        {
            // Arrange
            var json = JsonDocument.Parse("{\"a\": 1000, \"b\": 2000, \"c\": 3000}");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(
                json,
                new PrettyPrinterOptions(new KeepIndentationOption(), 10));

            // Assert
            result.Should().Be(@"{ ""a"": 1000,
  ""b"": 2000,
  ""c"": 3000 }".Replace("\n", Environment.NewLine));
        }

        [Fact]
        public void ObjectIndentAmount()
        {
            // Arrange
            var json = JsonDocument.Parse("{\"a\": 1000, \"b\": 2000, \"c\": 3000}");

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(
                json,
                new PrettyPrinterOptions(new IndentAmountOption(4), 10));

            // Assert
            result.Should().Be(@"{
    ""a"": 1000,
    ""b"": 2000,
    ""c"": 3000
}".Replace("\n", Environment.NewLine));
        }

        [Fact]
        public void MildlyComplex()
        {
            // Arrange
            dynamic obj = new ExpandoObject();
            obj.age = 20;
            obj.name = "John";
            obj.address = new ExpandoObject();
            obj.address.street = "Main";
            obj.address.number = 20;
            obj.address.city = "Smallville";
            obj.array = new [] { 1000, 2000, 3000, 4000, 5000, 6000, 7000, 8000, 9000, 10000 };
            string jsonStr = JsonSerializer.Serialize(obj);
            var json = JsonDocument.Parse(jsonStr);

            // Act
            var result = FancyPen.Json.PrettyPrinter.Print(
                json,
                new PrettyPrinterOptions(new IndentAmountOption(4), 60));

            // Assert
            result.Should().Be(@"{
    ""age"": 20,
    ""name"": ""John"",
    ""address"": {
        ""street"": ""Main"", ""number"": 20, ""city"": ""Smallville""
    },
    ""array"": [
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
}".Replace("\n", Environment.NewLine));
        }
    }
}