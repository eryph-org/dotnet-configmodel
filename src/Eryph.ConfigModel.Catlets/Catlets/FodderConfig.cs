

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets
{
    [PublicAPI]
    public class FodderConfig: ICloneable
    {
        public string? Name { get; set; }
        public bool? Remove { get; set; }
        public string? Type { get; set; }
        
        [PrivateIdentifier(Critical = true)]
        public string? Content { get; set; }
        public string? FileName { get; set; }

        public bool? Secret { get; set; }

        public FodderConfig Clone()
        {
            return new FodderConfig
            {
                Name = Name,
                Remove = Remove,
                Type = Type,
                Content = Content,
                FileName = FileName,
                Secret = Secret
            };
        }
        
        object ICloneable.Clone()
        {
            return Clone();
        }

        internal static FodderConfig[]? Breed(CatletConfig parentConfig, CatletConfig childConfig)
        {
            if (parentConfig.Fodder == null && childConfig.Fodder == null)
                return default;
            
            var newConfigs = parentConfig.Fodder?
                .Select(a=>a.Clone()).ToArray();
            
            var mergedConfig = new List<FodderConfig>();

            if (newConfigs != null)
            {
                mergedConfig.AddRange(newConfigs);
                
                foreach (var fodder in newConfigs)
                {
                    if (fodder.Remove.GetValueOrDefault())
                        mergedConfig.Remove(fodder);

                    var childFodder = childConfig.Fodder?
                        .FirstOrDefault(x => x.Name == fodder.Name);

                    if (childFodder == null)
                        continue;

                    if (childFodder.Remove.GetValueOrDefault())
                    {
                        mergedConfig.Remove(fodder);
                        continue;
                    }

                    fodder.Secret = childFodder.Secret ?? fodder.Secret;
                    fodder.Content = childFodder.Content ?? fodder.Content;
                    fodder.Type = childFodder.Type ?? fodder.Type;
                    fodder.FileName = childFodder.FileName ?? fodder.FileName;
                }
            }

            var parentNames = parentConfig.Fodder
                ?.Select(x => x.Name) ?? Array.Empty<string>();
            mergedConfig.AddRange(childConfig.Fodder?.Where(cfg =>
                                      !cfg.Remove.GetValueOrDefault() &&                      
                                      !parentNames.Any(x =>
                                          string.Equals(x, cfg.Name, StringComparison.InvariantCultureIgnoreCase))) 
                                  ?? Array.Empty<FodderConfig>());

            return mergedConfig.OrderBy(x => x.Name).ToArray();

        }
    }
}