using System;
using System.Runtime.Serialization;

namespace Eryph.ConfigModel
{
    [Serializable]
    public class InvalidConfigModelException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public InvalidConfigModelException()
        {
        }

        public InvalidConfigModelException(string message) : base(message)
        {
        }

        public InvalidConfigModelException(string message, Exception inner) : base(message, inner)
        {
        }

        protected InvalidConfigModelException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
}