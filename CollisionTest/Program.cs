using System.Linq;

using SFML.Window;
using SFML.Graphics;

using Latte.Core;
using Latte.Core.Type;
using Latte.Application;
using Latte.Debugging;

using Milkway.Physics;


namespace Milkway.Tests;


static class CollisionTestProgram
{
    private static PhysicsWorld PhysicsWorld { get; set; }
    private static RectPlayer Player { get; set; }

    private static Vec2f? WallStart { get; set; }
    private static Vec2f? WallEnd { get; set; }


    static CollisionTestProgram()
    {
        PhysicsWorld = new PhysicsWorld
        {
            Drag = new Vec2f(1.5f, 1.5f),
            Gravity = new Vec2f(y: 0.2f)
        };

        Player = new RectPlayer();

        PhysicsWorld.AddBody(Player);
    }


    private static void Main()
    {
        Engine.InitFullScreen("Milkway Engine - Collision Test");


        App.AddObjects(Player);

        while (!App.ShouldQuit)
        {
            App.Update();

            UpdateWallInsertion();
            UpdateWallRemoval();

            PhysicsWorld.Update();


            App.Window.Clear();
            App.Draw();

            DrawWallInsertion();
            DrawWallSelection();

            App.Window.Display();
        }
    }


    private static void UpdateWallInsertion()
    {
        if (Mouse.IsButtonPressed(Mouse.Button.Left))
        {
            if (WallStart is null)
                WallStart = MouseInput.PositionInView;

            WallEnd = MouseInput.PositionInView;
            return;
        }

        if (WallStart is null || WallEnd is null)
            return;

        var body = new RectBody(WallStart, WallEnd - WallStart) { Static = true };

        PhysicsWorld.AddBody(body);
        App.AddObject(body);

        WallStart = WallEnd = null;
    }


    private static void UpdateWallRemoval()
    {
        foreach (var body in PhysicsWorld.Bodies.ToArray())
        {
            if (body is not RectBody wall || body is RectPlayer)
                continue;

            if (MouseInput.PositionInView.IsPointOverObject(wall) && Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                PhysicsWorld.RemoveBody(wall);
                App.RemoveObject(wall);
            }
        }
    }


    private static void DrawWallInsertion()
    {
        if (WallStart is null || WallEnd is null)
            return;

        Draw.LineRect(App.Window.Renderer, new FloatRect(WallStart, WallEnd - WallStart), Color.White);
    }


    private static void DrawWallSelection()
    {
        foreach (var body in PhysicsWorld.Bodies)
        {
            if (body is not RectBody wall || body is RectPlayer)
                continue;

            if (MouseInput.PositionInView.IsPointOverObject(wall))
                Draw.LineRect(App.Window.Renderer, wall.BoundingBox(), Color.Red, 2f);
        }
    }
}
