namespace Eryph.ConfigModel.Catlets;

public interface IMutateableConfig<out T>
{
    string? Name { get; }
    MutationType? Mutation { get; }
    T Clone();
}