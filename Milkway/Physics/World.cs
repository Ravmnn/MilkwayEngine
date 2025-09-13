using System;
using System.Collections.Generic;

using Latte.Core;
using Latte.Core.Type;


namespace Milkway.Physics;


public class World : IUpdateable
{
    private readonly List<IBody> _bodies = [];

    public IEnumerable<IBody> Bodies => _bodies;

    public Vec2f Gravity { get; set; } = new Vec2f();
    public Vec2f Drag { get; set; } = new Vec2f();

    public event EventHandler? UpdateEvent;


    public void Update()
    {
        foreach (var body in Bodies)
            body.UpdateDisplacement();

        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }


    public void AddBody(IBody body)
    {
        body.World = this;
        _bodies.Add(body);
    }


    public bool RemoveBody(IBody body)
    {
        body.World = null;
        return _bodies.Remove(body);
    }
}
