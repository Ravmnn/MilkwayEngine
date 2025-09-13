using SFML.Graphics;

using Latte.Core.Objects;
using Latte.Core.Type;

using Milkway.Physics;


namespace Milkway.Tests;


public class RectBody(Vec2f position, Vec2f size, float radius = 0) : RectangleObject(position, size, radius), IBody
{
    public World? World { get; set; }

    public Vec2f Velocity { get; set; } = new Vec2f();
    public Vec2f Acceleration { get; set; } = new Vec2f();


    public FloatRect BoundingBox()
        => GetBounds();
}
