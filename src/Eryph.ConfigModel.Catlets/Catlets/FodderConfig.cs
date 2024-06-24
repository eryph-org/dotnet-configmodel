using System;
using System.Collections.Generic;
using System.Linq;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]
public class FodderConfig: ICloneableConfig<FodderConfig>, IHasVariableConfig
{
    public string? Name { get; set; }
    public bool? Remove { get; set; }
        
    public string? Source { get; set; }
    public string? Type { get; set; }
        
    [PrivateIdentifier(Critical = true)]
    public string? Content { get; set; }
    public string? FileName { get; set; }
        
    public bool? Secret { get; set; }

    public VariableConfig[]? Variables { get; set; }

    public FodderConfig Clone()
    {
        return new FodderConfig
        {
            Name = Name,
            Remove = Remove,
            Source = Source,
            Type = Type,
            Content = Content,
            FileName = FileName,
            Secret = Secret,
            Variables = Variables?.Select(x => x.Clone()).ToArray(),
        };
    }

    internal static FodderConfig[]? Breed(
        CatletConfig parentConfig, 
        CatletConfig childConfig,
        string? parentReference)
    {
        if (parentConfig.Fodder == null && childConfig.Fodder == null)
            return null;
            
        var newConfigs = parentConfig.Fodder?.Select(a=>a.Clone()).ToArray();
            
        var mergedConfig = new List<FodderConfig>();

        if (newConfigs != null)
        {
            mergedConfig.AddRange(newConfigs);
                
            foreach (var fodder in newConfigs)
            {

                if (string.IsNullOrWhiteSpace(fodder.Source))
                {
                    fodder.Source = $"gene:{parentReference}:{fodder.Name}";
                }
                    
                var childFodder = childConfig.Fodder?.FirstOrDefault(x => x.Name == fodder.Name);

                if (childFodder == null)
                    continue;

                if (childFodder.Remove.GetValueOrDefault() 
                    && !string.IsNullOrWhiteSpace(fodder.Name) && 
                    childFodder.Name == fodder.Name)
                {
                    mergedConfig.Remove(fodder);
                    continue;
                }
                    
                if (!string.IsNullOrWhiteSpace(childFodder.Content))
                    fodder.Source = null;
                    
                fodder.Secret = childFodder.Secret ?? fodder.Secret;
                fodder.Content = childFodder.Content ?? fodder.Content;
                fodder.Type = childFodder.Type ?? fodder.Type;
                fodder.FileName = childFodder.FileName ?? fodder.FileName;
                fodder.Remove = childFodder.Remove ?? fodder.Remove;

                // A parameterized fodder content is only useful with its corresponding
                // variables. Hence, we take the variables from the fodder config which
                // provides the content or the source.
                fodder.Variables = childFodder.Content is not null || childFodder.Source is not null
                    ? childFodder.Variables?.Select(x => x.Clone()).ToArray()
                    : fodder.Variables?.Select(x => x.Clone()).ToArray();
            }
        }

        if (childConfig.Fodder is not null)
        {
            var parentNames = new HashSet<string>(
                parentConfig.Fodder?.Select(x => x.Name ?? "") ?? [],
                StringComparer.OrdinalIgnoreCase);

            mergedConfig.AddRange(childConfig.Fodder
                .Where(cfg => !parentNames.Contains(cfg.Name ?? ""))
                .Select(x => x.Clone()));
        }

        return mergedConfig.ToArray();
    }
}
