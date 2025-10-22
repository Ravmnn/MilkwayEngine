using Latte.Core.Type;


namespace PathTracer2D.Engine;




public class RectangleObject : Object
{
    public RectangleObject(Vec2f position, Vec2f size)
    {
        Segments = [
            new Segment(this, position, position + new Vec2f(size.X, 0)),
            new Segment(this, position + new Vec2f(size.X, 0), position + size),
            new Segment(this, position + size, position + new Vec2f(0, size.Y)),
            new Segment(this, position + new Vec2f(0, size.Y), position)
        ];
    }
}
