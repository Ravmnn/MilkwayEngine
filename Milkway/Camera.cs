using System;

using SFML.Graphics;

using Latte.Core;
using Latte.Core.Type;


namespace Milkway;


public class Camera : IUpdateable
{
    private View _view;


    public RenderTarget Target { get; }

    public View View
    {
        get => _view;
        set
        {
            _view = value;

            Target.SetView(_view);
        }
    }

    public View LastView { get; set; }

    public Vec2f Position
    {
        get => View.Center - View.Size;
        set => View.Center = value + View.Size;
    }

    public Vec2f Size
    {
        get => View.Size * 2f;
        set => View.Size = value / 2f;
    }

    public Vec2f CenterPosition
    {
        get => View.Center;
        set => View.Center = value;
    }

    public Vec2f CenterSize
    {
        get => View.Size;
        set => View.Size = value;
    }

    public Vec2f DeltaPosition => LastView.Center - View.Center;
    public Vec2f DeltaSize => LastView.Size - View.Size;

    public event EventHandler? UpdateEvent;


    public Camera(RenderTarget target)
    {
        Target = target;
        LastView = _view = target.GetView();
    }


    public virtual void Update()
    {
        LastView = new View(_view);

        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }
}
