using Latte.Core.Type;


namespace PathTracer2D.Engine;




public struct Material(NormalizedColorRGBA color, float spreading = 0.0f)
{
    public NormalizedColorRGBA Color { get; set; } = color;
    public float Spreading { get; set; } = spreading;
}
