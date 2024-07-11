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
        ValidateProperty(toValidate, c => c.Name, CatletName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Project, ProjectName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Store, DataStoreName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Environment, EnvironmentName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Parent, GeneSetIdentifier.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Location, StorageIdentifier.NewValidation, path)
        | ValidateCatletDriveConfigs(toValidate, path)
        | ValidateProperty(toValidate, c => c.Cpu, ValidateCatletCpuConfig, path)
        | ValidateProperty(toValidate, c => c.Memory, ValidateCatletMemoryConfig, path)
        | ValidateCatletCapabilityConfigs(toValidate, path)
        | ValidateCatletNetworkConfigs(toValidate, path)
        | ValidateCatletNetworkAdapterConfigs(toValidate, path)
        | ValidateCatletFodderConfigs(toValidate, path)
        | VariableConfigValidations.ValidateVariableConfigs(toValidate, path);

    private static Validation<ValidationIssue, Unit> ValidateCatletDriveConfigs(
        CatletConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.Drives,
            ValidateCatletDriveConfig, path, minCount: 0, maxCount: 64)
        from __ in ValidateProperty(toValidate, c => c.Drives,
            drive => Validations.ValidateDistinct(
                drive, d => CatletDriveName.NewValidation(d.Name), "drive name"),
            path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateCatletDriveConfig(
        CatletDriveConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, CatletDriveName.NewValidation, path, required: true)
        | ValidateProperty(toValidate, c => c.Store, DataStoreName.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Location, StorageIdentifier.NewValidation, path)
        | ValidateProperty(toValidate, c => c.Size, ValidateCatletDriveSize, path)
        | ValidateProperty(toValidate, c => c.Source, s => ValidateCatletDriveSource(s, toValidate.Type), path);
    
    private static Validation<Error, Unit> ValidateCatletDriveSize(int size) =>
        guard(size > 0, Error.New("The drive size must be positive.")).ToValidation()
        | guardnot(size > 64 * 1024, Error.New("The drive size must be at most 64 TiB.")).ToValidation();

    private static Validation<Error, Unit> ValidateCatletDriveSource(
        string source,
        CatletDriveType? driveType) =>
        source.StartsWith("gene:")
            ? from _ in guard((driveType ?? CatletDriveType.VHD) == CatletDriveType.VHD,
                  Error.New("The drive must be plain VHD when using a gene pool source."))
              from __ in GeneIdentifier.NewValidation(source)
              select unit
            : from _ in Validations.ValidateWindowsPath(source, "source")
                .ToEither()
                .MapLeft(errors => Error.New("The source must be a valid gene identifier or path.", Error.Many(errors)))
                .ToValidation()
              select unit;

    private static Validation<ValidationIssue, Unit> ValidateCatletCpuConfig(
        CatletCpuConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Count, ValidateCatletCpuCount, path);

    private static Validation<Error, Unit> ValidateCatletCpuCount(int count) =>
        guard(count > 0, Error.New("The number of CPUs must be positive.")).ToValidation()
        | guardnot(count > 240, Error.New("The number of CPUs must be at most 240.")).ToValidation();

    private static Validation<ValidationIssue, Unit> ValidateCatletMemoryConfig(
        CatletMemoryConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Maximum, ValidateCatletMemorySize, path)
        | ValidateProperty(toValidate, c => c.Minimum, ValidateCatletMemorySize, path)
        | ValidateProperty(toValidate, c => c.Startup, ValidateCatletMemorySize, path);

    private static Validation<Error, Unit> ValidateCatletMemorySize(int memorySize) =>
        guard(memorySize >= 128, Error.New("The memory size must be least 128 MiB.")).ToValidation()
        // For Linux guests, dynamic memory only works reliably when the memory size is a multiple of 128 MiB.
        | guard(memorySize % 128 == 0, Error.New("The memory size must be a multiple of 128 MiB.")).ToValidation()
        | guardnot(memorySize > 12 * 1024 * 1024, Error.New("The memory size must be at most 12 TiB."))
            .ToValidation();

    private static Validation<ValidationIssue, Unit> ValidateCatletFodderConfigs(
        CatletConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.Fodder, FodderConfigValidations.ValidateFodderConfig, path)
                  | ValidateList(toValidate, c => c.Fodder, ValidateCatletFodderConfig, path)
        from __ in ValidateProperty(toValidate, c => c.Fodder,
                       fodder => Validations.ValidateDistinct(fodder,
                           f => FodderKey.Create(f.Name, f.Source).ToValidation(), "fodder"),
                       path)
                   | ValidateProperty(toValidate, c => c.Fodder, FodderConfigValidations.ValidateNoMultipleTagsForGeneSet, path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateCatletFodderConfig(
        FodderConfig toValidate,
        string path = "") =>
        guard(toValidate.Remove.GetValueOrDefault()
              || notEmpty(toValidate.Content)
              || notEmpty(toValidate.Source),
                new ValidationIssue(path, "The content or source must be specified when adding fodder."))
            .ToValidation();

    private static Validation<ValidationIssue, Unit> ValidateCatletCapabilityConfigs(
        CatletConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.Capabilities, ValidateCatletCapabilityConfig, path)
        from __ in ValidateProperty(toValidate, c => c.Capabilities,
            drive => Validations.ValidateDistinct(
                drive, d => CatletCapabilityName.NewValidation(d.Name), "capability name"),
            path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateCatletCapabilityConfig(
        CatletCapabilityConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, CatletCapabilityName.NewValidation, path, required: true);

    private static Validation<ValidationIssue, Unit> ValidateCatletNetworkConfigs(
        CatletConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.Networks, ValidateCatletNetworkConfig, path)
        from __ in ValidateProperty(toValidate, c => c.Networks,
            drive => Validations.ValidateDistinct(
                drive, d => EryphNetworkName.NewValidation(d.Name), "network name"),
            path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateCatletNetworkConfig(
        CatletNetworkConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, EryphNetworkName.NewValidation, path, required: true)
        | ValidateProperty(toValidate, c => c.AdapterName, CatletNetworkAdapterName.NewValidation, path);

    private static Validation<ValidationIssue, Unit> ValidateCatletNetworkAdapterConfigs(
        CatletConfig toValidate,
        string path = "") =>
        from _ in ValidateList(toValidate, c => c.NetworkAdapters,
            // Hyper-V on Windows Server 2016 only supports up to 8 network adapters.
            ValidateCatletNetworkAdapterConfig, path, minCount: 0, maxCount: 8)
        from __ in ValidateProperty(toValidate, c => c.NetworkAdapters,
            drive => Validations.ValidateDistinct(
                drive, d => CatletNetworkAdapterName.NewValidation(d.Name), "network adapter name"),
            path)
        select unit;

    private static Validation<ValidationIssue, Unit> ValidateCatletNetworkAdapterConfig(
        CatletNetworkAdapterConfig toValidate,
        string path = "") =>
        ValidateProperty(toValidate, c => c.Name, CatletNetworkAdapterName.NewValidation, path, required: true);
}
