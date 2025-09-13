using SFML.Graphics;

using Latte.Core.Type;


namespace Milkway.Physics;


// TODO: add something like Engine.Init(), which initializes stuff specifically for game development


public interface IBody
{
    World? World { get; set; }

    Vec2f Position { get; set; }
    Vec2f Velocity { get; set; }
    Vec2f Acceleration { get; set; }


    void UpdateDisplacement()
    {
        if (World is not null)
        {
            Acceleration += World.Gravity;
            Acceleration -= World.Drag * Velocity / 100f;
        }

        Position += Velocity;
        Velocity += Acceleration;
        Acceleration = new Vec2f();
    }


    FloatRect BoundingBox();
}
