using Latte.Core.Type;


namespace RayTracer2D.Engine;




public readonly struct IntersectionPoint(Ray ray, float t, float u)
{
    public Ray Ray { get; init; } = ray;
    public float RayT { get; init; } = t;
    public float SegmentU { get; init; } = u;

    public Vec2f Point => Ray.At(RayT);
}
