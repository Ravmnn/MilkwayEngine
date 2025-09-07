using System;

using Latte.Exceptions;


namespace Milkway.Exceptions;


public class MilkwayException : LatteException
{
    public MilkwayException(string message) : base(message)
    {
    }

    public MilkwayException(string message, Exception inner) : base(message, inner)
    {
    }
}
