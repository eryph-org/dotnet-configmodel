using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class IpPoolConfig
    {
        public string Name { get; set; }
        
        [PrivateIdentifier]
        public string FirstIp { get; set; }
        
        [PrivateIdentifier]
        public string LastIp { get; set; }
    }
}