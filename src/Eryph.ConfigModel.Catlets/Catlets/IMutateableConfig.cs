namespace Eryph.ConfigModel.Catlets;

public interface IMutateableConfig<out T> : ICloneableConfig<T>
{
    string? Name { get; }

    MutationType? Mutation { get; }
}