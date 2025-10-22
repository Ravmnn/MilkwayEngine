using System;
using System.Collections.Generic;

using SFML.Graphics;

using Latte.Core;
using Latte.Core.Type;
using Latte.Application;


namespace Milkway.Physics;



// TODO: add drag to bodies surface


public class PhysicsWorld : IUpdateable
{
    public List<IBody> Bodies { get; private set; } = [];

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


    // TODO: use delta time in physics calculation

    protected virtual void ApplyInfluencesTo(IBody body)
    {
        const float DragFactor = 0.01f;
        var dt = (float)DeltaTime.Seconds;

        body.Acceleration += Gravity * dt;
        body.Acceleration -= Drag * body.Velocity * DragFactor * dt;
    }


    public void AddBody(IBody body)
    {
        body.PhysicsWorld = this;
        AddEventCallbacks(body);

        Bodies.Add(body);
    }


    public bool RemoveBody(IBody body)
    {
        body.PhysicsWorld = null;
        RemoveEventCallbacks(body);

        return Bodies.Remove(body);
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


    // TODO: fix collision tunneling

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
