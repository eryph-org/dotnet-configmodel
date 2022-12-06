using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletSubnetConfig
    {

        public string Name { get; set; }
        public string IpPool { get; set; }
    }
}