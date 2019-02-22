using System.Threading.Tasks;
using NSpec;
using Shouldly;

namespace TestApp.Tests
{
    public class describe_one : nspec
    {
        void it_should_be_true() { true.ShouldBeTrue(); }
        void it_should_be_true_expression() => true.ShouldBeTrue();
        async Task it_should_be_true_async() { true.ShouldBeTrue(); }
        async Task it_should_be_true_async_expression() => true.ShouldBeTrue();

        void given_some_context()
        {
            itAsync["should be true async expression"] = async () => true.ShouldBeTrue();
            itAsync["should be true async"] = async () => { true.ShouldBeTrue(); };

            it["should be true expression"] = () => true.ShouldBeTrue();
            it["should be true "] = () => { true.ShouldBeTrue(); };

            new Each<string, string>{
                {"1","One"},
                {"2","Two"},
                {"3","Three"}
            }.Do((input, expected) =>
            {
                context[$"given nested context {input}"] = () =>
                {
                    itAsync[$"should be true async expression nested {expected}"] = async () => true.ShouldBeTrue();
                    itAsync[$"should be true async nested {expected}"] = async () => { true.ShouldBeTrue(); };

                    it[$"should be true expression nested {expected}"] = () => true.ShouldBeTrue();
                    it[$"should be true nested {expected}"] = () => { true.ShouldBeTrue(); };
                };
            });
        }
    }
}