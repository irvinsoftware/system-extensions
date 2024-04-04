namespace Irvin.Extensions.Reflection
{
    public interface IMemberContainer
    {
        string Name { get; }
        object CreateNew();
    }
}