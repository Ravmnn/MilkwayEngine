using System;

using Latte.Core.Type;


namespace Milkway;


public static class Vector
{
    public static float Magnitude(this Vec2f vector)
        => MathF.Sqrt(MathF.Pow(vector.X, 2) + MathF.Pow(vector.Y, 2));


    public static Vec2f Normalize(this Vec2f vector)
    {
        var magnitude = vector.Magnitude();
        return magnitude == 0 ? new Vec2f() : vector / magnitude;
    }
}
