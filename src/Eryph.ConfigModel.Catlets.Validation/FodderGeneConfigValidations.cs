﻿using System;
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
using static Dbosoft.Functional.Validations.ComplexValidations;

namespace Eryph.ConfigModel;

#nullable enable

public static class FodderGeneConfigValidations
{
    public static Validation<ValidationIssue, Unit> ValidateFodderGeneConfig(
        FodderGeneConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, GeneName.NewValidation, path, required: true)
        | ValidateFodderConfigs(toValidate, path)
        | VariableConfigValidations.ValidateVariableConfigs(toValidate, path);

    private static Validation<ValidationIssue, Unit> ValidateFodderConfigs(
        FodderGeneConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.Fodder, ValidateFodderConfig, path, minCount: 1)
        from __ in FodderConfigValidations.ValidateFodderConfigs(toValidate, path)
                   | ValidateList(toValidate, c => c.Fodder, ValidateFodderConfigContent, path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateFodderConfig(
        FodderConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, FodderName.NewValidation, path, required: true)
        | ValidateProperty(toValidate, c => c.Source, ValidateSourceIsEmpty, path)
        | ValidateProperty(
            toValidate,
            c => c.Variables,
            _ => Fail<Error, VariableConfig[]>(Error.New("Variables are not supported here.")),
            path);

    private static Validation<ValidationIssue, Unit> ValidateFodderConfigContent(
        FodderConfig toValidate,
        string path = "") =>
        guard(toValidate.Remove.GetValueOrDefault() || notEmpty(toValidate.Content),
                new ValidationIssue(path, "The content must be specified when adding fodder."))
            .ToValidation();

    private static Validation<Error, string> ValidateSourceIsEmpty(
        string source) =>
        from _  in guardnot(notEmpty(source),
                Error.New("References are not supported in fodder genes. The source must be empty."))
            .ToValidation()
        select source;
}
