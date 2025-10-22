using Latte.Core.Type;


namespace PathTracer2D.Engine;




public struct Material
{
    public ColorRGBA Color { get; set; }
    public float Spreading { get; set; }
}
