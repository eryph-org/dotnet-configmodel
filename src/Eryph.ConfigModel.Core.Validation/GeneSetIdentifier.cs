using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

public class GeneSetIdentifier : ValidatingNewType<GeneSetIdentifier, string, OrdStringOrdinalIgnoreCase>
{
    public GeneSetIdentifier(string value) : base(value)
    {
        (Organization, GeneSet, Tag) = ValidOrThrow(
            from nonEmptyValue in Validations<GeneSetIdentifier>.ValidateNotEmpty(value)
            let parts = nonEmptyValue.Split('/')
            from _ in guard(parts.Length is 2 or 3, Error.New(
                    "The gene set identifier is malformed. It must be either org/geneset or org/geneset/tag"))
                .ToValidation()
            from orgName in OrganizationName.Validate(parts[0])
            from geneSetName in GeneSetName.Validate(parts[1])
            from tagName in parts.Length == 3
                ? TagName.Validate(parts[2])
                : Success<Error, TagName>(new TagName("latest"))
            select (orgName, geneSetName, tagName));
    }

    public OrganizationName Organization { get; private set; }

    public GeneSetName GeneSet { get; private set; }

    public TagName Tag{ get; private set; }
}
