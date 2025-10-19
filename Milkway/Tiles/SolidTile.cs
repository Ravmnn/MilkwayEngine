using System;

using SFML.Graphics;

using Latte.Core.Type;

using Milkway.Physics;


namespace Milkway.Tiles;




public class SolidTile : Tile, IBoxBody
{
    public PhysicsWorld? PhysicsWorld { get; set; }


    public Vec2f Velocity { get; set; } = new Vec2f();
    public Vec2f Acceleration { get; set; } = new Vec2f();

    public bool Static => true;
    public bool Phantom { get; set; }


    public event EventHandler? MoveVerticallyEvent;
    public event EventHandler? MoveHorizontallyEvent;
    public event EventHandler<RigidBodyEventArgs>? CollideEvent;




    public SolidTile(PhysicsWorld physicsWorld, TileSet tileSet, uint id) : base(tileSet, id)
    {
        physicsWorld.AddBody(this);
    }


    public SolidTile(PhysicsWorld physicsWorld, Tile tile) : this(physicsWorld, tile.TileSet, tile.Source.Id)
    {
        Position = tile.Position;
    }




    public FloatRect BoundingBox()
        => GetBounds();




    public void OnMoveVertically()
        => MoveHorizontallyEvent?.Invoke(this, EventArgs.Empty);

    public void OnMoveHorizontally()
        => MoveVerticallyEvent?.Invoke(this, EventArgs.Empty);

    public void OnCollide(IBoxBody other)
        => CollideEvent?.Invoke(this, new RigidBodyEventArgs(other));
}
