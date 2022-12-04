using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class VirtualCatletNetworkAdapterConfig
    {
        public string Name { get; set; }

        public string MacAddress { get; set; }
    }
}