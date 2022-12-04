using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class CatletRaisingConfig
    {
        public string Hostname { get; set; }

        public CloudInitConfig[] Config { get; set; }
    }

}