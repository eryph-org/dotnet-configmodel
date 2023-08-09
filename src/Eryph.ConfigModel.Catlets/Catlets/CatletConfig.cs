using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletConfig
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Environment { get; set; }
        public string Society { get; set; }
        public string SocialName { get; set; }
        public VirtualCatletConfig VCatlet { get; set; }

        public CatletNetworkConfig[] Networks { get; set; }
        

        public FodderConfig[] Fodder { get; set; }
    }
}