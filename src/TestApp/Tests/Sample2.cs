using System.Threading.Tasks;
using NSpec;
using Shouldly;

namespace TestApp.Tests
{
    public abstract class describe_two_base<T> : nspec
    {
        public void given_base_context()
        {
            it["should be true"] = () => true.ShouldBeTrue();
        }
    }
    public class describe_two : describe_two_base<bool>
    {
        public void given_context()
        {
            it["should be false"] = () => false.ShouldBeFalse();
        }
    }
}