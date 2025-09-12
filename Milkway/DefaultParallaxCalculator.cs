using System;


namespace Milkway;


public class DefaultParallaxCalculator : IParallaxCalculator
{
    public float MovementFactor { get; set; } = 0.01f;
    public float ScaleFactor { get; set; } = 0.1f;
    public float ShadeFactor { get; set; } = 0.1f;


    public float Movement(float depth)
        => MovementFactor * depth;

    public float Scale(float depth)
        => 1f - ScaleFactor * depth;

    public float Shade(float depth)
        => ShadeFactor * depth;
}
