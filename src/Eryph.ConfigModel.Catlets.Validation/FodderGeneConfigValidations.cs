using System;
using System.Collections.Generic;
using System.Text;
using Dbosoft.Functional.Validations;
using Eryph.ConfigModel.Catlets;
using Eryph.ConfigModel.FodderGenes;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using static Eryph.ConfigModel.Validations;
using static Dbosoft.Functional.Validations.ComplexValidations;

namespace Eryph.ConfigModel;

#nullable enable

public static class FodderGeneConfigValidations
{
    public static Validation<ValidationIssue, Unit> ValidateFodderGeneConfig(
        FodderGeneConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, ValidateFodderGeneName, path, required: true)
        | ValidateProperty(toValidate, c => c.Requires, ValidateRequires, path)
        | ValidateFodderConfigs(toValidate, path)
        | VariableConfigValidations.ValidateVariableConfigs(toValidate, path, allowRemove: false);

    private static Validation<Error, GeneName> ValidateFodderGeneName(string name) =>
        from validName in GeneName.NewValidation(name)
        from _ in guardnot(validName == GeneName.New("catlet"),
            Error.New("The gene name 'catlet' is reserved."))
        select validName;

    // An empty list must be rejected as the JSON serializer would write it
    // and older eryph versions reject the unknown property.
    private static Validation<Error, string[]> ValidateRequires(string[] requires) =>
        from _ in guard(requires.Length > 0,
                Error.New("The list of required genes must not be empty. Omit it instead."))
            .ToValidation()
        from __ in requires.ToSeq()
            .Map(GeneRequirement.NewValidation)
            .Sequence()
        from ___ in ValidateDistinct(requires, GeneRequirement.NewValidation, "gene requirement")
        select requires;

    private static Validation<ValidationIssue, Unit> ValidateFodderConfigs(
        FodderGeneConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.Fodder, ValidateFodderConfig, path, minCount: 1)
        from __ in ValidateProperty(toValidate, c => c.Fodder,
            fodder => ValidateDistinct(fodder, f => FodderName.NewValidation(f.Name), "fodder name"),
            path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateFodderConfig(
        FodderConfig toValidate,
        string path = "") =>
        from _ in ValidateProperty(toValidate, c => c.Name, FodderName.NewValidation, path, required: true)
                  | ValidateProperty(toValidate, c => c.Source, ValidateSourceIsEmpty, path)
                  | ValidateProperty(
                      toValidate,
                      c => c.Variables,
                      _ => Fail<Error, VariableConfig[]>(Error.New("Variables are not supported here.")),
                      path)
        from __ in FodderConfigValidations.ValidateFodderConfig(toValidate, path)
                   | guard(toValidate.Remove.GetValueOrDefault() || notEmpty(toValidate.Content),
                           new ValidationIssue(path, "The content must be specified when adding fodder."))
                       .ToValidation()
        select unit;

    private static Validation<Error, string> ValidateSourceIsEmpty(
        string source) =>
        from _  in guardnot(notEmpty(source),
                Error.New("References are not supported in fodder genes. The source must be empty."))
            .ToValidation()
        select source;
}
