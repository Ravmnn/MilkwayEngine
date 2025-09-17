using System;

using SFML.Graphics;

using Latte.Core.Objects;
using Latte.Core.Type;

using Milkway.Physics;


namespace Milkway.Tests;


public class RectBody(Vec2f position, Vec2f size, float radius = 0) : RectangleObject(position, size, radius), IBoxBody
{
    public PhysicsWorld? PhysicsWorld { get; set; }

    public Vec2f Velocity { get; set; } = new Vec2f();
    public Vec2f Acceleration { get; set; } = new Vec2f();

    public bool Static { get; set; }
    public bool Phantom { get; set; }

    public event EventHandler? MoveVerticallyEvent;
    public event EventHandler? MoveHorizontallyEvent;
    public event EventHandler<RigidBodyEventArgs>? CollideEvent;


    public FloatRect BoundingBox()
        => GetBounds();


    public void OnMoveVertically()
        => MoveVerticallyEvent?.Invoke(this, EventArgs.Empty);

    public void OnMoveHorizontally()
        => MoveHorizontallyEvent?.Invoke(this, EventArgs.Empty);


    public void OnCollide(IBoxBody other)
        => CollideEvent?.Invoke(this, new RigidBodyEventArgs(other));
}
