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
    public abstract class ExtendedNewType<NEWTYPE, A, ORD, VALIDATING>
        : NewType<NEWTYPE, A, True<A>, ORD>
        where NEWTYPE : ExtendedNewType<NEWTYPE, A, ORD, VALIDATING>
        where ORD : struct, Ord<A>
        where VALIDATING : struct, Validating<A>
        
    {
        public static readonly Func<A, NEWTYPE> New = IL.Ctor<A, NEWTYPE>();

        public ExtendedNewType(A value) : this(default(VALIDATING).Validate(value)) { }

        private ExtendedNewType(Validation<Error, A> value)
            : base(value.Match(
                Succ: v => v,
                Fail: errors => throw new ArgumentException("The validation failed", nameof(value), Error.Many(errors).ToErrorException())))
        { }

        private static A DoesValidate(A value)
        {
            return default(VALIDATING).Validate(value).Match(
                Succ: v => v,
                Fail: errors => throw new ArgumentException("The validation failed", nameof(value), Error.Many(errors).ToErrorException()));
        }

        private static A Foo(A value)
        {
            return value;
        }

        public static Validation<Error, NEWTYPE> NewValidation(A value) =>
            from _ in default(VALIDATING).Validate(value)
            select IL.Ctor<Validation<Error, A>, NEWTYPE>()(Success<Error, A>(value));
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
