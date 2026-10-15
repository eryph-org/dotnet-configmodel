using System;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

/// <summary>
/// Represents a gene which must be present when a fodder gene is used.
/// The format is <c>org/geneset</c> (any fodder gene of the gene set)
/// or <c>org/geneset:genename</c>. A tag is not allowed as the tag is
/// chosen by the catlet which references the required gene.
/// </summary>
public class GeneRequirement : EryphName<GeneRequirement>
{
    public GeneRequirement(string value) : base(value)
    {
        (Organization, GeneSet, GeneName) = ValidOrThrow(
            from nonEmptyValue in Validations<GeneRequirement>.ValidateNotEmpty(value)
            from _ in guardnot(nonEmptyValue.StartsWith("gene:", StringComparison.OrdinalIgnoreCase),
                    Error.New("The gene requirement must not start with 'gene:'. It must be org/geneset or org/geneset:genename."))
                .ToValidation()
            let parts = nonEmptyValue.Split(':')
            from __ in guard(parts.Length is 1 or 2,
                    Error.New("The gene requirement is malformed. It must be org/geneset or org/geneset:genename."))
                .ToValidation()
            let geneSetParts = parts[0].Split('/')
            from ___ in guardnot(geneSetParts.Length == 3,
                    Error.New("The gene requirement must not contain a tag. The tag is chosen by the catlet."))
                .ToValidation()
            from ____ in guard(geneSetParts.Length == 2,
                    Error.New("The gene requirement is malformed. It must be org/geneset or org/geneset:genename."))
                .ToValidation()
            from orgName in OrganizationName.NewValidation(geneSetParts[0])
            from geneSetName in GeneSetName.NewValidation(geneSetParts[1])
            from geneName in parts.Length == 2
                ? ConfigModel.GeneName.NewValidation(parts[1]).Map(n => Some(n))
                : Success<Error, Option<GeneName>>(None)
            select (orgName, geneSetName, geneName));
    }

    public OrganizationName Organization { get; }

    public GeneSetName GeneSet { get; }

    public Option<GeneName> GeneName { get; }

    /// <summary>
    /// Checks if the given gene satisfies this requirement. The tag of
    /// the gene is ignored.
    /// </summary>
    public bool IsSatisfiedBy(GeneIdentifier geneId) =>
        geneId.GeneSet.Organization == Organization
        && geneId.GeneSet.GeneSet == GeneSet
        && GeneName.Map(n => n == geneId.GeneName).IfNone(true);
}
