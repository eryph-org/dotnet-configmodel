using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Eryph.ConfigModel.Converters;

internal class StringParser(string yaml) : IParser
{
    private readonly IParser _innerParser = new Parser(new Scanner(new StringReader(yaml)));

    public bool MoveNext() => _innerParser.MoveNext();

    public ParsingEvent? Current => _innerParser.Current;

    public string ConsumeMappingAsString()
    {
        if (!this.Accept<MappingStart>(out var mappingStart))
            throw new InvalidOperationException("Expected mapping start");

        if (mappingStart.Style is not MappingStyle.Block)
            throw new InvalidOperationException("Expected block mapping");

        var endEvent = ConsumeThisAndNestedEvents();
        if (endEvent is not MappingEnd mappingEnd)
            throw new InvalidOperationException("Expected mapping end");

        return ExtractYaml(mappingStart.Start, mappingEnd.End);
    }

    public string ConsumeSequenceAsString()
    {
        if (!this.Accept<SequenceStart>(out var sequenceStart))
            throw new InvalidOperationException("Expected mapping start");

        if (sequenceStart.Style is not SequenceStyle.Block)
            throw new InvalidOperationException("Sequences are only supported with indentation style");

        var endEvent = ConsumeThisAndNestedEvents();
        if (endEvent is not SequenceEnd sequenceEnd)
            throw new InvalidOperationException("Expected mapping end");

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

        var substring = yaml.Substring(startIndex, endIndex - startIndex);
        var lines = substring.Split('\n').ToList();
        var linesToTake = lines[lines.Count - 1] == indentSpaces ? lines.Count - 1 : lines.Count;

        var fixesLines = lines.Take(linesToTake)
            .Select(l => l.StartsWith(indentSpaces) ? l.Substring(indentSpaces.Length) : l)
            .Select(l => l.All(c => c == ' ') ? "" :l)
            .ToList();

        return string.Join("\n", fixesLines);
    }
}
