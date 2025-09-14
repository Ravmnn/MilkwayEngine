using System;

using Latte.Core.Type;


namespace Milkway.Physics;


// TODO: add something like Engine.Init(), which initializes stuff specifically for game development


public interface IBody
{
    World? World { get; set; }

    Vec2f Position { get; set; }
    Vec2f Velocity { get; set; }
    Vec2f Acceleration { get; set; }

    bool Static { get; set; }

    event EventHandler? MoveVerticallyEvent;
    event EventHandler? MoveHorizontallyEvent;


    void UpdateDisplacement()
    {
        const float DragFactor = 0.01f;

        if (Static)
        {
            Velocity = Acceleration = new Vec2f();
            return;
        }

        if (World is not null)
        {
            Acceleration += World.Gravity;
            Acceleration -= World.Drag * Velocity * DragFactor;
        }

        Position.X += Velocity.X;
        OnMoveHorizontally();

        Position.Y += Velocity.Y;
        OnMoveVertically();

        Velocity += Acceleration;
        Acceleration = new Vec2f();
    }


    void OnMoveVertically();
    void OnMoveHorizontally();
}
