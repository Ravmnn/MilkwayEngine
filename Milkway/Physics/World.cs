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
        {
            ApplyInfluencesTo(body);
            body.UpdateDisplacement();
        }

        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }


    protected virtual void ApplyInfluencesTo(IBody body)
    {
        const float DragFactor = 0.01f;

        body.Acceleration += Gravity;
        body.Acceleration -= Drag * body.Velocity * DragFactor;
    }


    public void AddBody(IBody body)
    {
        body.World = this;
        AddEventCallbacks(body);

        _bodies.Add(body);
    }


    public bool RemoveBody(IBody body)
    {
        body.World = null;
        RemoveEventCallbacks(body);

        return _bodies.Remove(body);
    }


    private void AddEventCallbacks(IBody body)
    {
        body.MoveHorizontallyEvent += ProcessHorizontalCollisions;
        body.MoveVerticallyEvent += ProcessVerticalCollisions;
    }

    private void RemoveEventCallbacks(IBody body)
    {
        body.MoveHorizontallyEvent -= ProcessHorizontalCollisions;
        body.MoveVerticallyEvent -= ProcessVerticalCollisions;
    }


    private void ProcessHorizontalCollisions(object? body, EventArgs __)
    {
        if (body is not IRigidBody rigidBody)
            return;

        foreach (var other in Bodies)
        {
            if (other == rigidBody || other is not IRigidBody rigidOther)
                continue;

            if (!Collision.IsColliding(rigidBody.BoundingBox(), rigidOther.BoundingBox(), out var intersection))
                continue;

            if (rigidBody.Velocity.X > 0f)
                rigidBody.Position.X -= intersection.Width;

            else if (rigidBody.Velocity.X < 0f)
                rigidBody.Position.X += intersection.Width;

            rigidBody.Velocity.X = 0;
            rigidBody.Acceleration.X = 0;

            rigidBody.OnCollide(rigidOther);
            rigidOther.OnCollide(rigidBody);
        }
    }


    private void ProcessVerticalCollisions(object? body, EventArgs __)
    {
        if (body is not IRigidBody rigidBody)
            return;

        foreach (var other in Bodies)
        {
            if (other == rigidBody || other is not IRigidBody rigidOther)
                continue;

            if (!Collision.IsColliding(rigidBody.BoundingBox(), rigidOther.BoundingBox(), out var intersection))
                continue;

            if (rigidBody.Velocity.Y > 0f)
                rigidBody.Position.Y -= intersection.Height;

            else if (rigidBody.Velocity.Y < 0f)
                rigidBody.Position.Y += intersection.Height;

            rigidBody.Velocity.Y = 0;
            rigidBody.Acceleration.Y = 0;

            rigidBody.OnCollide(rigidOther);
            rigidOther.OnCollide(rigidBody);
        }
    }
}
