using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class IpPoolConfig
    {
        public string Name { get; set; }
        public string FirstIp { get; set; }
        public string LastIp { get; set; }
    }
}