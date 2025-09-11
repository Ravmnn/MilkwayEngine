namespace Milkway;


public interface IParallaxCalculator
{
    float MovementFactor(float depth);
    float Scale(float depth);
    float Shade(float depth);
}
