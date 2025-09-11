using System;


namespace Milkway;


public class DefaultParallaxCalculator : IParallaxCalculator
{
    public float MovementFactor(float depth)
        => 0.01f * depth;

    public float Scale(float depth)
        => 1f - 0.15f * depth;

    public float Shade(float depth)
        => MathF.Max(0, 0.15f * depth);
}
