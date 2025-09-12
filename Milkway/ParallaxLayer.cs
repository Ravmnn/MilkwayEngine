using System;

using Latte.Core;
using Latte.Core.Type;


using SfSprite = SFML.Graphics.Sprite;


namespace Milkway;


public class ParallaxLayer(SfSprite content, float depth, int relativePriority = 0) : IUpdateable, IDrawable
{
    public SfSprite Content { get; set; } = content;
    public float Depth { get; set; } = depth;
    public int RelativePriority { get; set; } = relativePriority;

    public float ExtraMovement { get; set; }
    public float ExtraScale { get; set; }
    public float ExtraShade { get; set; }

    public event EventHandler? UpdateEvent;
    public event EventHandler? DrawEvent;


    public virtual void Update()
    {
        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }


    public virtual void UpdateLayer(IParallaxCalculator calculator, Vec2f movementDelta)
    {
        var movement = calculator.Movement(Depth) + ExtraMovement;
        var scale = Math.Abs(calculator.Scale(Depth) + ExtraScale);
        var shade = 1f - Math.Clamp(calculator.Shade(Depth) + ExtraShade, 0f, 1f);
        var shadeColor = new NormalizedColorRGBA(shade, shade, shade);

        Content.Position -= movementDelta * movement;
        Content.Scale = new Vec2f(scale, scale);
        Content.Color = shadeColor;
    }


    public virtual void Draw(IRenderer target)
    {
        target.Render(Content);

        DrawEvent?.Invoke(this, EventArgs.Empty);
    }
}
