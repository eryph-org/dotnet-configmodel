using System;
using System.Collections.Generic;
using System.Text;
using Eryph.ConfigModel.Catlets;
using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;
using static Eryph.ConfigModel.ConfigValidations;

namespace Eryph.ConfigModel
{
    public static  class CatletConfigValidations
    {
        public static Validation<ValidationIssue, Unit> ValidateCatletConfig(CatletConfig toValidate, string path = "") =>
            validateProperty(toValidate, c => c.Project, path, ProjectName.NewValidation)
            | validateProperty(toValidate, c => c.Environment, path, EnvironmentName.NewValidation)
            | validateList(toValidate, c => c.Drives, path, ValidateCatletDriveConfig)
            | validateProperty(toValidate, c => c.Cpu, path, ValidateCatletCpuConfig);

        public static Validation<ValidationIssue, Unit> ValidateCatletDriveConfig(
            CatletDriveConfig toValidate,
            string path = "") =>
            validateProperty(toValidate, c => c.Name, path, CatletDriveName.NewValidation);

        public static Validation<ValidationIssue, Unit> ValidateCatletCpuConfig(
            CatletCpuConfig toValidate,
            string path = "") =>
            validateProperty(toValidate, c => c.Count, path, c => guard(c > 0, Error.New("The CPU count must be positive")).ToValidation());
    }
}
