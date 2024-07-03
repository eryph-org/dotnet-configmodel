using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dbosoft.Functional.Validations;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.Variables;
using LanguageExt;
using LanguageExt.Common;

using static Dbosoft.Functional.Validations.ComplexValidations;
using static Eryph.ConfigModel.Validations;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel;

internal static class FodderConfigValidations
{
    public static Validation<ValidationIssue, Unit> ValidateFodderConfigs(
        IHasFodderConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.Fodder, ValidateFodderConfig, path)
        from __ in ValidateProperty(toValidate, c => c.Fodder, ValidateNoDuplicateFodder, path)
        from ___ in ValidateProperty(toValidate, c => c.Fodder, ValidateNoMultipleTagsForGeneSet, path)
        select unit;

    public static Validation<ValidationIssue, Unit> ValidateFodderConfig(
        FodderConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, FodderName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Source, GeneIdentifier.NewValidation, path)
        | guard(notEmpty(toValidate.Name) || notEmpty(toValidate.Source),
                new ValidationIssue(path, "The name or source must be specified."))
            .ToValidation()
        | ValidateProperty(toValidate, c => c.Type,
            s => ValidateWhenAllowed(s, toValidate, ValidateFodderType, "fodder type"), path)
        | ValidateProperty(toValidate, c => c.Content,
            s => ValidateWhenAllowed(s, toValidate, Success<Error, string>, "content"), path)
        | ValidateProperty(toValidate, c => c.FileName,
            s => ValidateWhenAllowed(s, toValidate, s2 => ValidateFileName(s2, "file name"), "file name"), path)
        | ValidateProperty(toValidate, c => c.Secret,
            s => ValidateWhenAllowed(s, toValidate, Success<Error, bool?>, "secret flag"), path)
        | ValidateVariableConfigs(toValidate, path);

    private static Validation<Error, T> ValidateWhenAllowed<T>(
        T value,
        FodderConfig config,
        Func<T, Validation<Error, T>> validate,
        string valueName) =>
        from valueIsNotEmpty in Success<Error, bool>(value is not null && (value is not string s || notEmpty(s)))
        from _ in guardnot(valueIsNotEmpty && config.Remove.GetValueOrDefault(),
                          Error.New($"The {valueName} must not be specified when the fodder is removed."))
                      .ToValidation()
                  | guardnot(valueIsNotEmpty && notEmpty(config.Source),
                          Error.New($"The {valueName} must not be specified when the fodder is a reference."))
                      .ToValidation()
        from __ in validate(value)
        select value;
    
    private static Validation<Error, string> ValidateFodderType(
        string fodderType) =>
        from _ in guard(fodderType
                    is "cloud-boothook"
                    or "cloud-config"
                    or "cloud-config-archive"
                    or "include-once-url"
                    or "include-url"
                    or "part-handler"
                    or "shellscript"
                    or "upstart-job",
                Error.New("The fodder type is not supported."))
            .ToValidation()
        select fodderType;

    private static Validation<ValidationIssue, Unit> ValidateVariableConfigs(
        FodderConfig toValidate,
        string path) =>
        from _ in ValidateProperty(toValidate, c => c.Variables,
            v => ValidateVariablesNotForbidden(v, toValidate), path)
        from __ in VariableConfigValidations.ValidateVariableConfigs(toValidate, path)
            | ValidateList(toValidate, c => c.Variables,
                (v, p) => ValidateFodderReferenceVariableConfig(v, toValidate, p), path)
        select unit;

    private static Validation<Error, VariableConfig[]> ValidateVariablesNotForbidden(
        VariableConfig[] variableConfigs,
        FodderConfig fodderConfig) =>
        from _ in guardnot(variableConfigs.Length > 0 && fodderConfig.Remove.GetValueOrDefault(),
                          Error.New("The variables must not be specified when the fodder is removed."))
                      .ToValidation()
        select variableConfigs;

    private static Validation<ValidationIssue, Unit> ValidateFodderReferenceVariableConfig(
        VariableConfig toValidate,
        FodderConfig fodderConfig,
        string path) =>
        from __ in guardnot(notEmpty(fodderConfig.Source) && toValidate.Required is not null,
                           new ValidationIssue(
                               path,
                               "The required flag cannot be specified when the fodder is a reference."))
                       .ToValidation()
                   | guardnot(notEmpty(fodderConfig.Source) && toValidate.Type is not null,
                           new ValidationIssue(
                               path,
                               "The variable type cannot be specified when the fodder is a reference."))
                       .ToValidation()
        select unit;

    private static Validation<Error, Unit> ValidateNoDuplicateFodder(
        FodderConfig[] fodderConfigs) =>
        from fodderNames in fodderConfigs.ToSeq()
            .Map(fc =>
                from name in Optional(fc.Name)
                    .Filter(notEmpty)
                    .Map(n => FodderName.NewEither(n).ToValidation())
                    .Sequence()
                from source in Optional(fc.Source)
                    .Filter(notEmpty)
                    .Map(s => GeneIdentifier.NewEither(s).ToValidation())
                    .Sequence()
                select (Name: name, Source: source))
            .Sequence()
        from _ in fodderNames
            .GroupBy(identity)
            .Filter(g => g.Count() > 1)
            .Map(g => g.Key)
            .Map(k => k.Name.Match(
                Some: n => k.Source.Match(
                    Some: s => $"The fodder name '{n}' for source '{s}' is not unique.",
                    None: () => $"The fodder name '{n}' is not unique."),
                None: () => k.Source.Match(
                    Some: s => $"The fodder source '{s}' is not unique.",
                    None: () => "Fodder name and source are missing.")))
            .Map(m => Fail<Error, Unit>(Error.New(m)))
            .Sequence()
        select unit;

    private static Validation<Error, Unit> ValidateNoMultipleTagsForGeneSet(
        FodderConfig[] fodderConfigs) =>
        from geneIds in fodderConfigs.ToSeq()
            .Map(fc => Optional(fc.Source).Filter(notEmpty))
            .Somes()
            .Map(s => GeneIdentifier.NewEither(s).ToValidation())
            .Sequence()
        from _ in ValidateNoMultipleTagsForGeneSet(geneIds)
        select unit;

    public static Validation<Error, Unit> ValidateNoMultipleTagsForGeneSet(
        Seq<GeneIdentifier> geneIdentifiers) =>
        from _ in Success<Error, Unit>(unit)
        let duplicates = geneIdentifiers
            .Map(id => id.GeneSet)
            .Distinct()
            .ToLookup(setId => (setId.Organization, setId.GeneSet))
            .Filter(g => g.Count() > 1)
        from __ in duplicates
            .Map(g => (GeneSet: $"{g.Key.Organization}/{g.Key.GeneSet}",
                Tags: string.Join(", ", g.Map(id => id.Tag).Distinct().Map(t => $"'{t.Value}'"))))
            .Map(g => Fail<Error, Unit>(Error.New(
                $"The gene set '{g.GeneSet}' is specified with different tags ({g.Tags}).")))
            .Sequence()
        select unit;
}
