namespace DotNetTestNSpec.Domain.Library
{
    public interface IProxyFactory
    {
        INspecController Create(string testAssemblyPath);
    }
}
