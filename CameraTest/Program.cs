using System;

using SFML.Graphics;

using Latte.Core.Type;
using Latte.Core.Objects;
using Latte.Application;

using Milkway.Physics;


namespace Milkway.Tests;


public static class CameraTestProgram
{
    public static Camera Camera { get; }
    public static PhysicsWorld PhysicsWorld { get; }

    public static RectPlayer Player { get; }


    static CameraTestProgram()
    {
        Engine.InitFullScreen("Milkway Engine - Camera Test");


        Camera = new Camera(App.Window);
        PhysicsWorld = new PhysicsWorld
        {
            Drag = new Vec2f(2f, 2f)
        };

        Player = new RectPlayer();


        Camera.Follow = Player;
        Camera.SoftFollowAmount = new Vec2f(5f, 5f);
    }


    public static void CreateRandomSquares(int count, FloatRect area, float sizeStart, float sizeEnd)
    {
        var randomGenerator = new Random();

        var from = area.Position;
        var to = area.Position + area.Size;

        for (var i = 0; i < count; i++)
        {
            var position = new Vec2f(randomGenerator.Next((int)from.X, (int)to.X), randomGenerator.Next((int)from.Y, (int)to.Y));
            var size = randomGenerator.Next((int)sizeStart, (int)sizeEnd);

            App.AddObject(new RectangleObject(position, new Vec2f(size, size)));
        }
    }


    public static void Main()
    {
        PhysicsWorld.AddBody(Player);
        App.AddObject(Player);

        CreateRandomSquares(2000, new FloatRect(-3000f, -3000f, 12000f, 12000f), 10f, 50f);

        while (!App.ShouldQuit)
        {
            Update();
            Draw();
        }
    }


    public static void Update()
    {
        App.Update();

        Camera.Update();
        PhysicsWorld.Update();
    }


    public static void Draw()
    {
        App.Window.Clear();

        App.Draw();

        App.Window.Display();
    }
}
