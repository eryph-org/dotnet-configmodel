using System;
using System.Collections.Generic;
using System.Text;
using Dbosoft.Functional.DataTypes;
using LanguageExt;
using LanguageExt.ClassInstances;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

public class GeneSetIdentifier : EryphName<GeneSetIdentifier>
{
    public GeneSetIdentifier(string value) : base(value)
    {
        (Organization, GeneSet, Tag) = ValidOrThrow(
            from nonEmptyValue in Validations<GeneSetIdentifier>.ValidateNotEmpty(value)
            let parts = nonEmptyValue.Split('/')
            from _ in guard(parts.Length is 2 or 3, Error.New(
                    "The gene set identifier is malformed. It must be either org/geneset or org/geneset/tag."))
                .ToValidation()
            from orgName in OrganizationName.NewValidation(parts[0])
            from geneSetName in GeneSetName.NewValidation(parts[1])
            from tagName in parts.Length == 3
                ? TagName.NewValidation(parts[2])
                : Success<Error, TagName>(TagName.New("latest"))
            select (orgName, geneSetName, tagName));
    }

    public GeneSetIdentifier(OrganizationName organization, GeneSetName geneSet)
        : this($"{organization.Value}/{geneSet.Value}") { }

    public GeneSetIdentifier(OrganizationName organization, GeneSetName geneSet, TagName tag)
        : this($"{organization.Value}/{geneSet.Value}/{tag.Value}") { }


    public OrganizationName Organization { get; }

    public GeneSetName GeneSet { get; }

    public TagName Tag{ get; }

    /// <summary>
    /// Returns the string representation including the tag.
    /// </summary>
    public string ValueWithTag => $"{Organization.Value}/{GeneSet.Value}/{Tag.Value}";

    /// <summary>
    /// Returns the string representation without the tag.
    /// </summary>
    public string ValueWithoutTag => $"{Organization.Value}/{GeneSet.Value}";
}
