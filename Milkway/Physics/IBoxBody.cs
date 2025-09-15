using System;

using SFML.Graphics;


namespace Milkway.Physics;


public class RigidBodyEventArgs(IBoxBody boxBody) : EventArgs
{
    public IBoxBody BoxBody { get; } = boxBody;
}


public interface IBoxBody : IBody
{
    bool Phantom { get; set; }
    bool IsSolid => Static && !Phantom;

    event EventHandler<RigidBodyEventArgs> CollideEvent;


    FloatRect BoundingBox();


    void OnCollide(IBoxBody other);
}
