using SFML.Window;
using SFML.Graphics;

using Latte.Core.Type;
using Latte.Rendering;
using Latte.Application;

using RayTracer2D.Engine;


using MouseButtonEventArgs = Latte.Application.MouseButtonEventArgs;


namespace RayTracer2D;




public sealed class MainSection : Section
{
    private readonly RaySource _mouseLight;


    public Vec2u Resolution => new Vec2u(16 * 120, 9 * 120);
    public Vec2u WindowResolution => App.Window.Size;

    public Vec2f Scale => (Vec2f)WindowResolution / (Vec2f)Resolution;


    public RayTracer RayTracer { get; set; }


    public bool DebugDrawRayLines { get; set; }
    public bool DebugDrawSegmentLines { get; set; }
    public bool DebugDrawInfo { get; set; }




    public MainSection()
    {
        _mouseLight = new RaySource(new Vec2f(), 64);


        RayTracer = new RayTracer([], [_mouseLight]);


        for (var i = 0; i < 50; i++)
        {
            var generator = new Random();
            var position = new Vec2f(generator.Next(0, 1920), generator.Next(0, 1080));
            var size = new Vec2f(generator.Next(15, 200), generator.Next(5, 200));

            RayTracer.Segments.AddRange(RectangleSegment(position, size));
        }


        DebugDrawRayLines = false;
        DebugDrawSegmentLines = false;
        DebugDrawInfo = true;


        MouseInput.ButtonUpEvent += ProcessMouseInput;
    }


    private Segment[] RectangleSegment(Vec2f position, Vec2f size)
        => [
            new Segment(position, position + new Vec2f(size.X, 0)),
            new Segment(position + new Vec2f(size.X, 0), position + size),
            new Segment(position + size, position + new Vec2f(0, size.Y)),
            new Segment(position + new Vec2f(0, size.Y), position)
        ];




    public override void Update()
    {
        _mouseLight.Position = MouseInput.PositionInView;

        ProcessKeyInput();

        base.Update();
    }


    private void ProcessMouseInput(object? _, MouseButtonEventArgs args)
    {
        if (args.Button == Mouse.Button.Left)
            RayTracer.RaySources.Add(new RaySource(_mouseLight.Position, _mouseLight.RayCount));
    }


    private void ProcessKeyInput()
    {
        if (KeyboardInput.ReleasedKeyCode == Keyboard.Scancode.Num1)
            DebugDrawRayLines = !DebugDrawRayLines;

        if (KeyboardInput.ReleasedKeyCode == Keyboard.Scancode.Num2)
            DebugDrawSegmentLines = !DebugDrawSegmentLines;

        if (KeyboardInput.ReleasedKeyCode == Keyboard.Scancode.Num3)
            DebugDrawInfo = !DebugDrawInfo;


        if (KeyboardInput.ReleasedKeyCode == Keyboard.Scancode.NumpadPlus)
            _mouseLight.RayCount *= 2;

        if (KeyboardInput.ReleasedKeyCode == Keyboard.Scancode.NumpadMinus)
            _mouseLight.RayCount /= 2;
    }




    public override void Draw(IRenderer renderer)
    {
        // TODO: be able to correctly change the resolution without breaking the coordinate system
        var scene = RayTracer.Render(Resolution);
        var sprite = new Sprite(new Texture(scene));
        sprite.Scale = Scale;

        renderer.Render(sprite);


        DrawRaySources(renderer);
        DebugDraw(renderer);

        base.Draw(renderer);
    }


    private void DrawRaySources(IRenderer renderer)
    {
        foreach (var raySource in RayTracer.RaySources)
            Latte.Debugging.Draw.Point(renderer, raySource.Position);
    }


    private void DebugDraw(IRenderer renderer)
    {
        if (DebugDrawRayLines)
            DebugRayLines(renderer);

        if (DebugDrawSegmentLines)
            DebugSegments(renderer);

        if (DebugDrawInfo)
            DebugInfo(renderer);
    }


    private void DebugRayLines(IRenderer renderer)
    {
        var intersectionPoints = RayTracer.TraceAll();

        foreach (var ray in RayTracer.Rays)
            Latte.Debugging.Draw.Line(renderer, ray.Origin, ray.At(10000), Color.Red);

        foreach (var intersectionPoint in intersectionPoints)
            Latte.Debugging.Draw.Line(renderer, intersectionPoint.Ray.Origin, intersectionPoint.Point, Color.Blue);
    }


    private void DebugSegments(IRenderer renderer)
    {
        foreach (var segment in RayTracer.Segments)
            Latte.Debugging.Draw.Line(renderer, segment.Start, segment.End, Color.White);
    }


    private void DebugInfo(IRenderer renderer)
    {
        var info = $"FPS: {(int)DeltaTime.FramesPerSecond}\n" +
                   $"Current Rays: {_mouseLight.RayCount}";

        Latte.Debugging.Draw.Text(renderer, new Vec2f(), info, 12, Color.White);
    }
}
