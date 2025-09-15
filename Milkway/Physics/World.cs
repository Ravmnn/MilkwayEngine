using System;
using System.Collections.Generic;

using SFML.Graphics;

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


    private void ProcessHorizontalCollisions(object? sender, EventArgs __)
    {
        if (sender is IBody body)
            ProcessStaticCollisionsOf(body, (rigidBody, intersection) =>
            {
                if (rigidBody.Velocity.X > 0f)
                    rigidBody.Position.X -= intersection.Width;

                else if (rigidBody.Velocity.X < 0f)
                    rigidBody.Position.X += intersection.Width;

                rigidBody.Velocity.X = 0;
                rigidBody.Acceleration.X = 0;
            });
    }


    private void ProcessVerticalCollisions(object? sender, EventArgs __)
    {
        if (sender is IBody body)
            ProcessStaticCollisionsOf(body, (rigidBody, intersection) =>
            {
                if (rigidBody.Velocity.Y > 0f)
                    rigidBody.Position.Y -= intersection.Height;

                else if (rigidBody.Velocity.Y < 0f)
                    rigidBody.Position.Y += intersection.Height;

                rigidBody.Velocity.Y = 0;
                rigidBody.Acceleration.Y = 0;
            });
    }


    private void ProcessStaticCollisionsOf(IBody body, Action<IBoxBody, FloatRect> collisionResponse)
    {
        if (body is not IBoxBody boxBody)
            return;

        foreach (var other in Bodies)
        {
            if (other == boxBody || other is not IBoxBody boxOther)
                continue;

            if (!Collision.IsColliding(boxBody.BoundingBox(), boxOther.BoundingBox(), out var intersection))
                continue;

            // static bodies can't naturally update their position, so they don't reach this part.

            if (!boxBody.Phantom && boxOther.IsSolid)
                collisionResponse(boxBody, intersection);

            boxBody.OnCollide(boxOther);
            boxOther.OnCollide(boxBody);
        }
    }
}
