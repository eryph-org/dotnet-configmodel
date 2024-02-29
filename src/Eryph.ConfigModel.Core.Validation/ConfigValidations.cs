using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using LanguageExt;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public static class ConfigValidations
    {
        public static Validation<ValidationIssue, Unit> validateProperty<T, TProperty, TResult>(
            T toValidate,
            Expression<Func<T, TProperty>> getProperty,
            string path,
            Func<TProperty, Validation<Error, TResult>> validate) =>
            Optional(getProperty.Compile().Invoke(toValidate))
                .Filter(o => o is not string s || notEmpty(s))
                .Match(
                    Some: v => validate(v).Map(_ => unit)
                        .MapFail(e => new ValidationIssue(joinPath(path, getProperty), e.Message)),
                    None: Success<ValidationIssue, Unit>(unit));

        public static Validation<ValidationIssue, Unit> validateProperty<T, TProperty>(
            T toValidate,
            Expression<Func<T, TProperty>> getProperty,
            string path,
            Func<TProperty, string, Validation<ValidationIssue, Unit>> validate) =>
            Optional(getProperty.Compile().Invoke(toValidate))
                .Match(
                    Some: v => validate(v, joinPath(path, getProperty)),
                    None: Success<ValidationIssue, Unit>(unit));

        public static Validation<ValidationIssue, Unit> validateList<T, TProperty>(
            T toValidate,
            Expression<Func<T, IEnumerable<TProperty>>> getList,
            string path,
            Func<TProperty, string, Validation<ValidationIssue, Unit>> validate) => 
            getList.Compile().Invoke(toValidate).ToSeq()
                .Map((index, listItem) => validate(listItem, $"{joinPath(path, getList)}[{index}]"))
                .Fold(Success<ValidationIssue, Unit>(unit), (acc, listItem) => acc | listItem);
        
        private static string joinPath<T,TProperty>(string path, Expression<Func<T, TProperty>> getProperty)
        {
            if (getProperty.Body is not MemberExpression memberExpression)
                throw new ArgumentException("The expression must access and return a class member");

            return notEmpty(path) ? $"{path}.{memberExpression.Member.Name}" : memberExpression.Member.Name;
        }
    }
}
