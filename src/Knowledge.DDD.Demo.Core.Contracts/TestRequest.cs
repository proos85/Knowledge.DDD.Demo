using MediatR;

namespace Knowledge.DDD.Demo.Core.Contracts;

public sealed class TestRequest: IRequest
{
    public TestRequest(int number)
    {
        Number = number;
    }

    public int Number { get; }
}