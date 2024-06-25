using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

        var childFodderByKey = (childConfig.Fodder ?? [])
            .ToDictionary(CreateFodderKey);

        var parentFodderKeys = new HashSet<(string, string)>(
            (parentConfig.Fodder ?? []).Select(CreateFodderKey));
            
        var mergedConfig = new List<FodderConfig>();

        foreach (var parentFodder in parentConfig.Fodder ?? [])
        {
            var mergedFodder = parentFodder.Clone();

            if (string.IsNullOrWhiteSpace(mergedFodder.Source)
                && !string.IsNullOrWhiteSpace(parentReference))
            {
                mergedFodder.Source = $"gene:{parentReference}:catlet";
            }

            if (childFodderByKey.TryGetValue(CreateFodderKey(parentFodder), out var childFodder))
            {
                if (childFodder.Remove.GetValueOrDefault()
                    && string.IsNullOrWhiteSpace(parentFodder.Source) 
                    && string.IsNullOrWhiteSpace(childFodder.Source)
                    && string.Equals(parentFodder.Name, childFodder.Name, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                mergedFodder.Secret = childFodder.Secret ?? parentFodder.Secret;
                mergedFodder.Content = childFodder.Content ?? parentFodder.Content;
                mergedFodder.Type = childFodder.Type ?? parentFodder.Type;
                mergedFodder.FileName = childFodder.FileName ?? parentFodder.FileName;
                mergedFodder.Remove = childFodder.Remove ?? parentFodder.Remove;

                // A parameterized fodder content is only useful with its corresponding
                // variables. Hence, we take the variables from the fodder config which
                // provides the content or the source.
                mergedFodder.Variables = childFodder.Content is not null || childFodder.Source is not null
                    ? childFodder.Variables?.Select(x => x.Clone()).ToArray()
                    : parentFodder.Variables?.Select(x => x.Clone()).ToArray();
            }

            mergedConfig.Add(mergedFodder);
        }

        mergedConfig.AddRange((childConfig.Fodder ?? [])
            .Where(cfg => !parentFodderKeys.Contains(CreateFodderKey(cfg)))
            .Select(x => x.Clone()));

        return mergedConfig.ToArray();
    }


    private static (string Name, string Source) CreateFodderKey(FodderConfig fodder)
    {
        return (string.IsNullOrWhiteSpace(fodder.Name) ? null : fodder.Name.ToUpperInvariant(),
                string.IsNullOrWhiteSpace(fodder.Source) ? null : fodder.Source.ToUpperInvariant());
    }
}
