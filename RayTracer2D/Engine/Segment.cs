using Latte.Core.Type;


namespace RayTracer2D.Engine;




public struct Segment(Vec2f start, Vec2f end)
{
    public Vec2f Start { get; set; } = start;
    public Vec2f End { get; set; } = end;

    public Vec2f Vector => End - Start;




    public Vec2f At(float u)
        => Start + (End - Start) * u; // u is normalized, 0 to 1
}
