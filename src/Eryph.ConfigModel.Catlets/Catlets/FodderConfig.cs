

using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class FodderConfig
    {
        public string Name { get; set; }
        public string Type { get; set; }
        
        [PrivateIdentifier(Critical = true)]
        public string Content { get; set; }
        public string FileName { get; set; }

        public bool Secret { get; set; }
    }
}