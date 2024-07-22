using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

/// <summary>
/// This record is used to identify a fodder (reference) for
/// deduplication. It makes use of <see cref="FodderName"/>
/// and <see cref="GeneIdentifier"/> which handle the respective
/// normalization.
/// </summary>
public readonly record struct FodderKey
{
    private FodderKey(
        Option<FodderName> name,
        Option<GeneIdentifier> source)
    {
        Name = name;
        Source = source;
    }

    public Option<FodderName> Name { get; }

    public Option<GeneIdentifier> Source { get; }

    public override string ToString()
    {
        var name = Name;
        return Source.Match(
            Some: s => $"{s}{name.Map(n => $"->{n.Value}").IfNone("")}",
            None: () => $"catlet->{name.Map(n => n.Value).IfNone("")}");
    }

    public static Either<Error, FodderKey> Create(Option<string> name, Option<string> source) =>
        from validName in name.Filter(notEmpty)
            .Map(FodderName.NewEither)
            .Sequence()
            .MapLeft(e => Error.New("Found fodder with invalid name."))
        from validSource in source.Filter(notEmpty)
            .Map(GeneIdentifier.NewEither)
            .Sequence()
            .MapLeft(e => Error.New("Found fodder with invalid source."))
        from _ in guardnot(validName.IsNone && validSource.IsNone,
            Error.New("Found invalid fodder with neither name nor source."))
        // The breeding injects informational sources for fodder taken from
        // the parent (which uses the gene name 'catlet'). These sources must
        // be ignored during deduplication.
        let isFodderGene = validSource.Filter(s => s.GeneName != GeneName.New("catlet")).IsSome
        from __ in guardnot(!isFodderGene && validName.IsNone,
            Error.New($"Found catlet fodder '{validSource.Map(s => s.Value).IfNone("")}' without valid name."))
        select isFodderGene ? new FodderKey(validName, validSource) : new FodderKey(validName, None);
}
