using SFML.Window;
using SFML.Graphics;

using Latte.Core.Type;
using Latte.Application;

using Milkway.Physics;


namespace Milkway.Tests;


static class CollisionTestProgram
{
    private static World World { get; set; }

    private static RectBody Player { get; set; }


    static CollisionTestProgram()
    {
        World = new World
        {
            Drag = new Vec2f(3f, 3f),
            Gravity = new Vec2f(y: 0.1f)
        };

        Player = new RectBody(new Vec2f(100, 100), new Vec2f(30, 30))
        {
            Color = Color.Red
        };

        World.AddBody(Player);
    }


    private static void Main()
    {
        App.Init(VideoMode.FullscreenModes[0], "Milkway Engine - Collision Test", style: Styles.Fullscreen);


        App.AddObject(Player);

        while (!App.ShouldQuit)
        {
            UpdateMovement();

            World.Update();

            App.Update();
            App.Draw();
        }
    }


    private static void UpdateMovement()
    {
        const float AccelerationFactor = 0.5f;

        if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            Player.Acceleration.Y = -AccelerationFactor;

        if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            Player.Acceleration.Y = AccelerationFactor;

        if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            Player.Acceleration.X = -AccelerationFactor;

        if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            Player.Acceleration.X = AccelerationFactor;
    }
}
