using System;
using System.IO;
using System.Linq;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Eryph.ConfigModel.Yaml;

/// <summary>
/// This class wraps the actual <see cref="IParser"/> for a <see cref="string"/>.
/// Its purpose is to provide access to slices of parsed string while it
/// is processed by the YAML parser.
/// </summary>
public class StringParser(string yaml) : IParser
{
    private readonly IParser _innerParser = new Parser(new StringReader(yaml));

    public bool MoveNext() => _innerParser.MoveNext();

    public ParsingEvent? Current => _innerParser.Current;

    /// <summary>
    /// Consumes the mapping at the current location and returns the
    /// corresponding slice from the input string.
    /// </summary>
    public string ConsumeMappingAsString()
    {
        if (!this.Accept<MappingStart>(out var mappingStart))
            throw new InvalidOperationException("A mapping should start at this point.");

        var endEvent = ConsumeThisAndNestedEvents();

        if (endEvent is not MappingEnd mappingEnd)
            throw new InvalidOperationException("The mapping should end at this point.");

        if (mappingStart.Style is not MappingStyle.Block)
            throw new YamlException(
                mappingStart.Start,
                mappingEnd.End,
                "Only indentation style mappings are supported at this point.");

        return ExtractYaml(mappingStart.Start, mappingEnd.End);
    }

    /// <summary>
    /// Consumes the sequence at the current location and returns the
    /// corresponding slice from the input string.
    /// </summary>
    public string ConsumeSequenceAsString()
    {
        if (!this.Accept<SequenceStart>(out var sequenceStart))
            throw new InvalidOperationException("A sequence should start at this point.");

        var endEvent = ConsumeThisAndNestedEvents();

        if (endEvent is not SequenceEnd sequenceEnd)
            throw new InvalidOperationException("The sequence should end at this point.");

        if (sequenceStart.Style is not SequenceStyle.Block)
            throw new YamlException(
                sequenceStart.Start,
                sequenceEnd.End,
                "Only indentation style sequences are supported at this point.");

        return ExtractYaml(sequenceStart.Start, sequenceEnd.End);
    }

    private ParsingEvent ConsumeThisAndNestedEvents()
    {
        var depth = 0;
        ParsingEvent parsingEvent;
        do
        {
            parsingEvent = this.Consume<ParsingEvent>();
            depth += parsingEvent.NestingIncrease;
        } while (depth > 0);

        return parsingEvent;
    }

    private string ExtractYaml(Mark start, Mark end)
    {
        var startIndex = (int)start.Index;
        var startIndent = (int)start.Column - 1;
        var endIndex = (int)end.Index;

        var indentSpaces = new string(' ', startIndent);

        // Apply the same sanitization to the line breaks as YamlDotNet does.
        var yamlSlice = yaml.Substring(startIndex, endIndex - startIndex)
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Replace("\x85", "\n");

        var lines = yamlSlice.Split('\n')
            // Remove the part of the indentation which belongs to the mapping or
            // sequence which we are consuming.
            .Select(l => l.StartsWith(indentSpaces) ? l.Substring(indentSpaces.Length) : l)
            // Trim any lines which exclusively consist of spaces (from the indentation).
            .Select(l => l.All(c => c == ' ') ? "" : l);

        var result = string.Join("\n", lines);

        // Always terminate the result with a line break. This way, the correct
        // multiline string literal is used when serializing the data again.
        return result.EndsWith("\n") ? result : $"{result}\n";
    }
}
