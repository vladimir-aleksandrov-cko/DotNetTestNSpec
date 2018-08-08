namespace DotNetTestNSpec.Domain.Library
{
    public interface IProxyFactory
    {
        IController Create(string testAssemblyPath);
    }
}
