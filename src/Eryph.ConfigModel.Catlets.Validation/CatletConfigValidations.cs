using System;
using System.Collections.Generic;
using System.Text;
using Dbosoft.Functional.Validations;
using Eryph.ConfigModel.Catlets;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using static Dbosoft.Functional.Validations.ComplexValidations;

namespace Eryph.ConfigModel;

public static  class CatletConfigValidations
{
    public static Validation<ValidationIssue, Unit> ValidateCatletConfig(CatletConfig toValidate, string path = "") =>
        ValidateProperty(toValidate, c => c.Project, ProjectName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Environment, EnvironmentName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Parent, GeneSetIdentifier.NewValidation, path)
        | ValidateList(toValidate, c => c.Drives, ValidateCatletDriveConfig, path, minCount: 0, maxCount: 64)
        | ValidateProperty(toValidate, c => c.Cpu, ValidateCatletCpuConfig, path)
        | ValidateProperty(toValidate, c => c.Memory, ValidateCatletMemoryConfig, path);

    public static Validation<ValidationIssue, Unit> ValidateCatletDriveConfig(
        CatletDriveConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, CatletDriveName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Size, ValidateCatletDriveSize, path)
        | ValidateProperty(toValidate, c => c.Source, source =>
            source.StartsWith("gene:")
                ? GeneIdentifier.NewValidation(source).Map(_ => unit)
                : Validations.ValidatePath(source, "source").Map(_ => unit)
                    .ToEither()
                    .MapLeft(errors => Error.New("The source must be a valid gene identifier or path.", Error.Many(errors)))
                    .ToValidation(),
            path);
    
    private static Validation<Error, Unit> ValidateCatletDriveSize(int size) =>
        guard(size > 0, Error.New("The drive size must be positive.")).ToValidation()
        | guardnot(size > 64 * 1024, Error.New("The drive size must be at most 64 TB.")).ToValidation();

    public static Validation<ValidationIssue, Unit> ValidateCatletCpuConfig(
        CatletCpuConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Count, ValidateCatletCpuCount, path);

    private static Validation<Error, Unit> ValidateCatletCpuCount(int count) =>
        guard(count > 0, Error.New("The number of CPUs must be positive.")).ToValidation()
        | guardnot(count > 240, Error.New("The number of CPUs must be at most 240.")).ToValidation();

    public static Validation<ValidationIssue, Unit> ValidateCatletMemoryConfig(
        CatletMemoryConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Maximum, ValidateCatletMemorySize, path)
        | ValidateProperty(toValidate, c => c.Minimum, ValidateCatletMemorySize, path)
        | ValidateProperty(toValidate, c => c.Startup, ValidateCatletMemorySize, path);

    private static Validation<Error, Unit> ValidateCatletMemorySize(int memorySize) =>
        guard(memorySize >= 32, Error.New("The memory size must be least 32 MB.")).ToValidation()
        | guard(memorySize % 2 == 0, Error.New("The memory size must be a multiple of 2 MB.")).ToValidation()
        | guardnot(memorySize > 12 * 1024 * 1024, Error.New("The memory size must be at most 12 TB,"))
            .ToValidation();
}
