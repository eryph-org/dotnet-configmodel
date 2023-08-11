using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Eryph.ConfigModel.Catlets;

internal static class Breeding
{
    public static TSubConfig[]? WithMutation<TConfig,TSubConfig>(
        TConfig parentConfig, TConfig childConfig, 
        Expression<Func<TConfig, TSubConfig[]?>> listProperty,
        Action<TSubConfig, TSubConfig> mergeAction,
        Action<TSubConfig>? adoptAction = default) 
        where TSubConfig: IMutateableConfig<TSubConfig>
    {
        var accessorFunc = listProperty.Compile();
        var parentList = accessorFunc(parentConfig);
        var childList = accessorFunc(childConfig);
            
        if (childList == null && parentList == null)
            return default;
            
        var newConfigs = parentList?.Select(a=>a.Clone()).ToArray();
        if (childList == null || newConfigs == null)
            return newConfigs?
                .Select(c =>
                {
                    adoptAction?.Invoke(c);
                    return c;
                }).ToArray();
            
        //merge settings configured both on parent and child
        var mergedConfig = new List<TSubConfig>();
        mergedConfig.AddRange(newConfigs);
            
        foreach (var parentSubConfig in newConfigs)
        {
            if(parentSubConfig.Mutation.GetValueOrDefault() == MutationType.Remove)
                mergedConfig.Remove(parentSubConfig);
            
            adoptAction?.Invoke(parentSubConfig);
            
            var childSubConfig = childList
                .FirstOrDefault(x => x.Name == parentSubConfig.Name);

            if (childSubConfig == null)
                continue;
                
            switch (childSubConfig.Mutation)
            {
                case MutationType.Overwrite:
                    mergedConfig.Remove(parentSubConfig);
                    mergedConfig.Add(childSubConfig);
                    break;
                case MutationType.Remove:
                    mergedConfig.Remove(parentSubConfig);
                    break;
                case MutationType.Merge:
                    goto default;
                case null:
                    goto default;
                default:
                    mergeAction(parentSubConfig, childSubConfig);
                    break;
            }
        }
            
        //add adapters configured only on child
        var childNames = childList.Select(x => x.Name);
        mergedConfig.AddRange(parentList?.Where(vmHd =>
                                  vmHd.Mutation != MutationType.Remove &&                      
                                  !childNames.Any(x =>
                                      string.Equals(x, vmHd.Name, StringComparison.InvariantCultureIgnoreCase))) 
                              ?? Array.Empty<TSubConfig>());

        return mergedConfig.OrderBy(x => x.Name).ToArray();
            
    }
            
}