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
    public abstract class ExtendedNewType<NEWTYPE, A, PRED, ORD>(A value)
        : NewType<NEWTYPE, A, PRED, ORD>(value)
        where NEWTYPE : ExtendedNewType<NEWTYPE, A, PRED, ORD>
        where PRED : struct, PredWithMessage<A>
        where ORD : struct, Ord<A>
    {
        public static Validation<Error, NEWTYPE> NewValidation(A value)
        {
            return match(NewTry(value),
                Succ: Success<Error, NEWTYPE>,
                Fail: ex => Fail<Error, NEWTYPE>(default(PRED).Message));
        }
    }

    public interface PredWithMessage<A> : Pred<A>
    {
        string Message { get; }
    }

}
