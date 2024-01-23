using Eryph.ConfigModel.Catlets;

namespace Eryph.ConfigModel;

public interface IHasFodderConfig
{
    public FodderConfig[]? Fodder { get; set; }

}