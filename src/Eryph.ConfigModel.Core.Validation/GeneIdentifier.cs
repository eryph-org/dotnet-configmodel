using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

public class GeneIdentifier : ValidatingNewType<GeneIdentifier, string, OrdStringOrdinalIgnoreCase>
{
    public GeneIdentifier(string value) : base(value)
    {
        (GeneSet, GeneName) = ValidOrThrow(
            from nonEmptyValue in Validations<GeneSetIdentifier>.ValidateNotEmpty(value)
            let parts = nonEmptyValue.Split(':')
            from _ in guard(parts.Length is 3 && parts[0] == "gene", Error.New(
                    "The gene identifier is malformed. It must be gene:geneset:genename"))
                .ToValidation()
            from geneSetIdentifier in GeneSetIdentifier.Validate(parts[1])
            from geneName in GeneName.Validate(parts[2])
            select (geneSetIdentifier, geneName));
    }

    public GeneSetIdentifier GeneSet { get; private set; }

    public GeneName GeneName { get; private set; }
}
