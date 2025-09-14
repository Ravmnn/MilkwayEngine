using System;

using SFML.Graphics;


namespace Milkway.Physics;


public class RigidBodyEventArgs(IRigidBody rigidBody) : EventArgs
{
    public IRigidBody RigidBody { get; } = rigidBody;
}


public interface IRigidBody : IBody
{
    event EventHandler<RigidBodyEventArgs> CollideEvent;


    FloatRect BoundingBox();


    void OnCollide(IRigidBody other);
}
