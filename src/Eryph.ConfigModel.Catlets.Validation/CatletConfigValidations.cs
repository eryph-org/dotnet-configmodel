using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Catlets;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using static Eryph.ConfigModel.ConfigValidations;

namespace Eryph.ConfigModel;

public static  class CatletConfigValidations
{
    public static Validation<ValidationIssue, Unit> ValidateCatletConfig(CatletConfig toValidate, string path = "") =>
        validateProperty(toValidate, c => c.Project, path, ProjectName.Validate)
        | validateProperty(toValidate, c => c.Environment, path, EnvironmentName.Validate)
        | validateProperty(toValidate, c => c.Parent, path, GeneSetIdentifier.Validate)
        | validateList(toValidate, c => c.Drives, path, ValidateCatletDriveConfig)
        | validateProperty(toValidate, c => c.Cpu, path, ValidateCatletCpuConfig)
        | validateProperty(toValidate, c => c.Memory, path, ValidateCatletMemoryConfig);

    public static Validation<ValidationIssue, Unit> ValidateCatletDriveConfig(
        CatletDriveConfig toValidate,
        string path = "") =>
        validateProperty(toValidate, c => c.Name, path, CatletDriveName.Validate)
        | validateProperty(toValidate, c => c.Size, path, ValidateCatletDriveSize)
        | validateProperty(toValidate, c => c.Source, path, source =>
            source.StartsWith("gene:")
                ? GeneIdentifier.Validate(source).Map(_ => unit)
                : Validations.ValidatePath(source, "source").Map(_ => unit)
                    .ToEither()
                    .MapLeft(errors => Error.New("The source must be a valid gene identifier or path.", Error.Many(errors)))
                    .ToValidation());
    
    private static Validation<Error, Unit> ValidateCatletDriveSize(int? size) =>
        guard(size > 0, Error.New("The drive size must be positive.")).ToValidation()
        | guardnot(size > 64 * 1024, Error.New("The drive size must be at most 64 TB.")).ToValidation();

    public static Validation<ValidationIssue, Unit> ValidateCatletCpuConfig(
        CatletCpuConfig toValidate,
        string path = "") =>
        validateProperty(toValidate, c => c.Count, path, ValidateCatletCpuCount);

    private static Validation<Error, Unit> ValidateCatletCpuCount(int? count) =>
        guard(count > 0, Error.New("The number of CPUs must be positive.")).ToValidation()
        | guardnot(count > 240, Error.New("The number of CPUs must be at most 240.")).ToValidation();

    public static Validation<ValidationIssue, Unit> ValidateCatletMemoryConfig(
        CatletMemoryConfig toValidate,
        string path = "") =>
        validateProperty(toValidate, c => c.Maximum, path, ValidateCatletMemorySize)
        | validateProperty(toValidate, c => c.Minimum, path, ValidateCatletMemorySize)
        | validateProperty(toValidate, c => c.Startup, path, ValidateCatletMemorySize);

    private static Validation<Error, Unit> ValidateCatletMemorySize(int? memorySize) =>
        guard(memorySize >= 32, Error.New("The memory size must be least 32 MB.")).ToValidation()
        | guard(memorySize % 2 == 0, Error.New("The memory size must be a multiple of 2 MB.")).ToValidation()
        | guardnot(memorySize > 12 * 1024 * 1024, Error.New("The memory size must be at most 12 TB,"))
            .ToValidation();
}
