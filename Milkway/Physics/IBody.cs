using System;

using Latte.Application;
using Latte.Core.Type;


namespace Milkway.Physics;




public interface IBody
{
    PhysicsWorld? PhysicsWorld { get; set; }


    Vec2f Position { get; set; }
    Vec2f Velocity { get; set; }
    Vec2f Acceleration { get; set; }

    bool Static { get; }


    event EventHandler? MoveVerticallyEvent;
    event EventHandler? MoveHorizontallyEvent;




    void UpdateDisplacement()
    {
        if (Static)
        {
            Velocity = Acceleration = new Vec2f();
            return;
        }

        var dt = (float)DeltaTime.Seconds;

        Position.X += Velocity.X * dt;
        OnMoveHorizontally();

        Position.Y += Velocity.Y * dt;
        OnMoveVertically();

        Velocity += Acceleration;
        Acceleration = new Vec2f();
    }




    void OnMoveVertically();
    void OnMoveHorizontally();
}
