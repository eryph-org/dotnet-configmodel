﻿using System;
using System.Linq;
using Eryph.ConfigModel.Variables;
using JetBrains.Annotations;

namespace Eryph.ConfigModel.Catlets;

[PublicAPI]
public class CatletConfig : ICloneableConfig<CatletConfig>, IHasFodderConfig, IHasVariableConfig
{
    public string? Version { get; set; }
    public string? Project { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }
    public string? Hostname { get; set; }
    public string? Environment { get; set; }
    public string? Store { get; set; }
    public string? Parent { get; set; }
        
    public CatletCpuConfig? Cpu { get; set; }

    public CatletMemoryConfig? Memory { get; set; }

    public CatletDriveConfig[]? Drives { get; set; }

    public CatletNetworkAdapterConfig[]? NetworkAdapters { get; set; }
        
    public CatletCapabilityConfig[]? Capabilities  { get; set; }

    public CatletNetworkConfig[]? Networks { get; set; }

    public VariableConfig[]? Variables { get; set; }

    public FodderConfig[]? Fodder { get; set; }

    public CatletConfig Clone() => new()
    {
        Version = Version,
        Project = Project,
        Name = Name,
        Location = Location,
        Hostname = Hostname,
        Environment = Environment,
        Store = Store,
        Parent = Parent,
        Cpu = Cpu?.Clone(),
        Networks = Networks?.Select(x=>x.Clone()).ToArray(),
        Capabilities = Capabilities?.Select(x=>x.Clone()).ToArray(),
        Drives = Drives?.Select(x=>x.Clone()).ToArray(),
        Fodder = Fodder?.Select(x=>x.Clone()).ToArray(),
        NetworkAdapters = NetworkAdapters?.Select(x=>x.Clone()).ToArray(),
        Memory = Memory?.Clone(),
        Variables = Variables?.Select(x=>x.Clone()).ToArray(),
    };
}
