using System;

using SFML.Graphics;

using Latte.Core.Objects;
using Latte.Core.Type;


namespace Milkway.Physics;




public class RectangleBody(Vec2f position, Vec2f size)
    : RectangleObject(position, size), IBoxBody
{
    public PhysicsWorld? PhysicsWorld { get; set; }


    public Vec2f Velocity { get; set; } = new Vec2f();
    public Vec2f Acceleration { get; set; } = new Vec2f();

    public bool Static { get; set; } = true;
    public bool Phantom { get; set; }


    public event EventHandler? MoveVerticallyEvent;
    public event EventHandler? MoveHorizontallyEvent;
    public event EventHandler<RigidBodyEventArgs>? CollideEvent;




    public virtual FloatRect BoundingBox()
        => GetBounds();




    public virtual void OnMoveVertically()
        => MoveVerticallyEvent?.Invoke(this, EventArgs.Empty);

    public virtual void OnMoveHorizontally()
        => MoveHorizontallyEvent?.Invoke(this, EventArgs.Empty);


    public virtual void OnCollide(IBoxBody other)
        => CollideEvent?.Invoke(this, new RigidBodyEventArgs(other));
}
