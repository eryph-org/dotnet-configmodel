using LanguageExt.ClassInstances.Pred;
using LanguageExt.ClassInstances;
using LanguageExt.TypeClasses;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Text;
using LanguageExt.Common;
using static LanguageExt.Prelude;

namespace Eryph.ConfigModel
{
    public abstract class ValidatingNewType<NEWTYPE, A, ORD>(A value)
        : NewType<NEWTYPE, A, True<A>, ORD>(value)
        where NEWTYPE : ValidatingNewType<NEWTYPE, A, ORD>
        where ORD : struct, Ord<A>
        //where VALIDATING : struct, Validating<A>
    {
        public static Validation<Error, NEWTYPE> Validate(A value) =>
            NewTry(value).Match(
                Succ: Success<Error, NEWTYPE>,
                Fail: ex => ex switch
                {
                    ManyExceptions mex => Fail<Error, NEWTYPE>(mex.Errors.Map(e => e.ToError())),
                    ErrorException eex => Fail<Error, NEWTYPE>(eex.ToError()),
                    _ => Fail<Error, NEWTYPE>(Error.New("The validation failed", ex))
                });

        protected static T ValidOrThrow<T>(Validation<Error, T> validation) =>
            validation.Match(
                Succ: identity,
                Fail: errors => Error.Many(errors).ToErrorException().Rethrow<T>());
    }

    public interface PredWithMessage<A> : Pred<A>
    {
        string Message { get; }
    }

    public interface ValidatingPred<A> : Pred<A>
    {
        Validation<Error, A> Validate(A value);
    }

    public interface Validating<A>
    {
        Validation<Error, A> Validate(A value);
    }

}
