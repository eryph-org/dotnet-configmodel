using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

/// <summary>
/// Represents a gene identifier. The tag <c>latest</c> is automatically
/// added when no tag has been specified for the gene set.
/// </summary>
public class GeneIdentifier : EryphName<GeneIdentifier>
{
    public GeneIdentifier(string value) : base(Normalize(value))
    {
        (GeneSet, GeneName) = ValidOrThrow(
            from nonEmptyValue in Validations<GeneSetIdentifier>.ValidateNotEmpty(value)
            let parts = nonEmptyValue.Split(':')
            from _ in guard(parts.Length is 3 && parts[0] == "gene", Error.New(
                    "The gene identifier is malformed. It must be gene:geneset:genename"))
                .ToValidation()
            from geneSetIdentifier in GeneSetIdentifier.NewValidation(parts[1])
            from geneName in GeneName.NewValidation(parts[2])
            select (geneSetIdentifier, geneName));
    }

    public GeneIdentifier(GeneSetIdentifier geneSetIdentifier, GeneName geneName)
        : this($"gene:{geneSetIdentifier.Value}:{geneName.Value}") { }

    public GeneSetIdentifier GeneSet { get; }

    public GeneName GeneName { get; }

    private static string Normalize(string value) => 
        Optional(value)
            .Filter(v => v.Count(c => c == ':') == 2 && v.Count(c => c == '/') == 1)
            .Match(Some: v => v.Insert(v.LastIndexOf(':'), "/latest"),
                   None: () => value);
}
