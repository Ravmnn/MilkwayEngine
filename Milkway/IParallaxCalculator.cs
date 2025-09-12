namespace Milkway;


public interface IParallaxCalculator
{
    public float MovementFactor { get; set; }
    public float ScaleFactor { get; set; }
    public float ShadeFactor { get; set; }


    float Movement(float depth);
    float Scale(float depth);
    float Shade(float depth);
}
