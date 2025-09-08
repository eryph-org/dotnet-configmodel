namespace Eryph.ConfigModel.Catlets;

public enum CatletConfigType
{
    /// <summary>
    /// This configuration is a normal catlet configuration.
    /// It can contain unresolved genes.
    /// User-provided configurations have this type by default.
    /// </summary>
    Configuration = 0,

    /// <summary>
    /// This configuration belongs to a catlet specification. It
    /// contains only resolved genes and has been normalized.
    /// </summary>
    Specification = 1,
    
    /// <summary>
    /// This configuration describes a concrete instance of a catlet.
    /// It contains information specific to the instance:
    /// MAC addresses, storage locations, etc.
    /// </summary>
    Instance = 2,
}
