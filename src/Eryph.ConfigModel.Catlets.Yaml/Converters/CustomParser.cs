using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;

namespace Eryph.ConfigModel.Converters;

internal class CustomParser : IParser
{
    private readonly IParser _innerParser;
    private readonly IScanner _scanner;
    private readonly string _yaml;

    public CustomParser(string yaml)
    {
        _yaml = yaml;
        _scanner = new Scanner(new StringReader(yaml));
        _innerParser = new Parser(_scanner);
    }

    public bool MoveNext() => _innerParser.MoveNext();

    public ParsingEvent? Current => _innerParser.Current;

    public string ConsumeMappingAsString()
    {
        if (!this.Accept<MappingStart>(out var mappingStart))
            throw new InvalidOperationException("Expected mapping start");

        var endEvent = ConsumeThisAndNestedEvents();
        if (endEvent is not MappingEnd mappingEnd)
            throw new InvalidOperationException("Expected mapping end");

        return ExtractYaml(mappingStart.Start, mappingEnd.End);
    }

    public string ConsumeSequenceAsString()
    {
        if (!this.Accept<SequenceStart>(out var sequenceStart))
            throw new InvalidOperationException("Expected mapping start");

        var endEvent = ConsumeThisAndNestedEvents();
        if (endEvent is not SequenceStart sequenceEnd)
            throw new InvalidOperationException("Expected mapping end");

        return ExtractYaml(sequenceStart.Start, sequenceEnd.End);
    }

    public string SkipAndReturnString()
    {
        if (!this.TryConsume<MappingStart>(out var mappingStart))
            throw new InvalidOperationException("Expected mapping start");

        int depth = mappingStart.NestingIncrease;
        ParsingEvent parsingEvent;
        do
        {
            parsingEvent = this.Consume<ParsingEvent>();
            depth += parsingEvent.NestingIncrease;
        } while (depth > 0);

        if (parsingEvent is not MappingEnd mappingEnd)
            throw new InvalidOperationException("Expected mapping end");

        var begin = (int)mappingStart.Start.Index;
        var end = (int)mappingEnd.End.Index;

        var indent = (int)mappingStart.Start.Column - 1;

        return AdjustIndentation(_yaml.Substring(begin, end - begin), indent);
    }

    private ParsingEvent ConsumeThisAndNestedEvents()
    {
        int depth = 0;
        ParsingEvent parsingEvent;
        do
        {
            parsingEvent = this.Consume<ParsingEvent>();
            depth += parsingEvent.NestingIncrease;
        } while (depth > 0);

        return parsingEvent;
    }


    private string AdjustIndentation(string yaml, int indent)
    {
        var lines = yaml.Split('\n');

        return string.Join("\n", yaml.Split('\n').Select(l => l.Substring(indent)));
    }

    private string ExtractYaml(Mark start, Mark end)
    {
        var startIndex = (int)start.Index;
        var startIndent = (int)start.Column - 1;
        var endIndex = (int)end.Index;

        var indentSpaces = new string(' ', startIndent);

        var lines = _yaml.Substring(startIndex, endIndex - startIndex).Split('\n').ToList();
        var linesToTake = lines[lines.Count - 1] == indentSpaces ? lines.Count - 1 : lines.Count;

        var fixesLines = lines.Take(linesToTake)
            .Select(l => l.StartsWith(indentSpaces) ? l.Substring(indentSpaces.Length) : l)
            .Select(l => l.All(c => c == ' ') ? "" :l)
            .ToList();

        return string.Join("\n", fixesLines);
    }
}
