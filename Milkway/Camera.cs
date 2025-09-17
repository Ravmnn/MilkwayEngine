using System;

using SFML.Graphics;

using Latte.Core;
using Latte.Core.Type;


namespace Milkway;


public class Camera : IUpdateable
{
    private Vec2f _softFollowAmount;


    public RenderTarget Target { get; }

    public View View { get; set; }
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

    public BaseObject? Follow { get; set; }

    public Vec2f SoftFollowAmount
    {
        get => _softFollowAmount;
        set => _softFollowAmount = new Vec2f(Math.Max(1f, value.X), Math.Max(1f, value.Y));
    }

    public event EventHandler? UpdateEvent;


    public Camera(RenderTarget target)
    {
        _softFollowAmount = null!;


        Target = target;
        LastView = View = target.GetView();

        SoftFollowAmount = new Vec2f(1f, 1f);
    }


    public virtual void Update()
    {
        UpdateFollowing();

        Target.SetView(View);

        LastView = new View(View);

        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }


    private void UpdateFollowing()
    {
        if (Follow is null)
            return;

        var objectCenterPosition = (Vec2f)(Follow.Position + Follow.GetBounds().Size / 2f);

        var distance = CenterPosition.Distance(objectCenterPosition);
        var direction = Vector.Normalize(objectCenterPosition - CenterPosition);

        CenterPosition += direction * distance / SoftFollowAmount;
    }
}
