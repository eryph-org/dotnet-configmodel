using System;

namespace Eryph.ConfigModel
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class PrivateIdentifierAttribute : Attribute
    {
        public bool Critical { get; set; }
    }
}