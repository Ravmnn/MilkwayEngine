using System.Linq;
using SFML.Window;
using SFML.Graphics;

using Latte.Core;
using Latte.Core.Type;
using Latte.Application;
using Latte.Debugging;

using Milkway.Physics;


namespace Milkway.Tests;


class PlayerType : RectBody
{
    public PlayerType() : base(new Vec2f(100, 100), new Vec2f(30, 30))
    {
        Color = SFML.Graphics.Color.Red;
    }
}


static class CollisionTestProgram
{
    private static World World { get; set; }
    private static PlayerType Player { get; set; }

    private static Vec2f? WallStart { get; set; }
    private static Vec2f? WallEnd { get; set; }


    static CollisionTestProgram()
    {
        World = new World
        {
            Drag = new Vec2f(1.5f, 1.5f),
            Gravity = new Vec2f(y: 0.2f)
        };

        Player = new PlayerType();

        World.AddBody(Player);
    }


    private static void Main()
    {
        App.Init(VideoMode.FullscreenModes[0], "Milkway Engine - Collision Test", style: Styles.Fullscreen);
        App.Debugger!.EnableKeyShortcuts = true;
        App.ManualObjectUpdate = false;
        App.ManualClearDisplayProcess = true;


        App.AddObjects(Player);

        while (!App.ShouldQuit)
        {
            App.Update();

            UpdateWallInsertion();
            UpdateWallRemoval();
            UpdateMovement();

            World.Update();


            App.Window.Clear();
            App.Draw();

            DrawWallInsertion();
            DrawWallSelection();

            App.Window.Display();
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

        World.AddBody(body);
        App.AddObject(body);

        WallStart = WallEnd = null;
    }


    private static void UpdateWallRemoval()
    {
        foreach (var body in World.Bodies.ToArray())
        {
            if (body is not RectBody wall || body is PlayerType)
                continue;

            if (MouseInput.PositionInView.IsPointOverObject(wall) && Mouse.IsButtonPressed(Mouse.Button.Right))
            {
                World.RemoveBody(wall);
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
        foreach (var body in World.Bodies)
        {
            if (body is not RectBody wall || body is PlayerType)
                continue;

            if (MouseInput.PositionInView.IsPointOverObject(wall))
                Draw.LineRect(App.Window.Renderer, wall.BoundingBox(), Color.Red, 2f);
        }
    }
}
