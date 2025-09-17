using SFML.Window;

using Latte.Core.Type;


namespace Milkway.Tests;


public class RectPlayer : RectBody
{
    public RectPlayer() : base(new Vec2f(100, 100), new Vec2f(30, 30))
    {
        Color = SFML.Graphics.Color.Red;
    }


    public override void Update()
    {
        const float AccelerationFactor = 0.5f;

        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            Acceleration.Y = -AccelerationFactor;

        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            Acceleration.Y = AccelerationFactor;

        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            Acceleration.X = -AccelerationFactor;

        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            Acceleration.X = AccelerationFactor;

        base.Update();
    }
}
