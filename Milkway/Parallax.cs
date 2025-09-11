using System;
using System.Collections.Generic;

using Latte.Core;
using Latte.Core.Type;


namespace Milkway;


public class Parallax : IUpdateable, IDrawable
{
    public Camera Camera { get; set; }

    public List<ParallaxLayer> Layers { get; set; }
    public IParallaxCalculator Calculator { get; set; }

    public bool Active { get; set; }

    public event EventHandler? UpdateEvent;
    public event EventHandler? DrawEvent;


    public Parallax(Camera camera, IParallaxCalculator? calculator = null)
    {
        Camera = camera;
        Layers = [];

        Calculator = calculator ?? new DefaultParallaxCalculator();

        Active = true;
    }


    public virtual void Update()
    {
        if (!Active)
            return;

        foreach (var (content, depth, _) in Layers)
        {
            var movementFactor = Calculator.MovementFactor(depth);
            var scale = Calculator.Scale(depth);

            content.Position -= Camera.DeltaPosition * movementFactor;
            content.Scale = new Vec2f(scale, scale);

            Console.WriteLine(Camera.DeltaPosition);

            // TODO: finish this
            // content.Color = ...;
        }

        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }


    public void Draw(IRenderer target)
    {
        SortLayersByDepthAndPriority();

        foreach (var layer in Layers)
            target.Render(layer.Content);

        DrawEvent?.Invoke(this, EventArgs.Empty);
    }


    private void SortLayersByDepthAndPriority()
    {
        Layers.Sort((a, b) =>
        {
            var priorityA = a.Depth - a.RelativePriority;
            var priorityB = b.Depth - b.RelativePriority;

            return priorityA.CompareTo(priorityB);
        });
        Layers.Reverse();
    }
}
