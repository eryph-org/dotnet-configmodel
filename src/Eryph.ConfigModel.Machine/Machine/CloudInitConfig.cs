

using JetBrains.Annotations;

namespace Eryph.ConfigModel.Machine
{
    [PublicAPI]
    public class CloudInitConfig
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }

        public bool Sensitive { get; set; }
    }
}