using Milkway.Physics;


namespace Milkway.Tiles;




public enum TileMapCollisionMethod
{
    All,
    Outline
}




public static class TileMapCollisionExtensions
{
    public static void AddStaticCollisionBodies(this TileMap tileMap, PhysicsWorld world, TileMapCollisionMethod collisionMethod)
    {
        switch (collisionMethod)
        {
            case TileMapCollisionMethod.All:
                AddStaticCollisionBodiesToAll(tileMap, world);
                break;

            case TileMapCollisionMethod.Outline:
                AddStaticCollisionBodiesToOutline(tileMap, world);
                break;
        }
    }


    private static void AddStaticCollisionBodiesToAll(TileMap tileMap, PhysicsWorld world)
    {
        foreach (var tile in tileMap.Tiles)
        {
            if (tile.Empty)
                continue;

            // NOTE: this is not being updated (BaseObject.Update) anywhere
            var body = new RectangleBody(tile.Position, tile.GetBounds().Size);
            world.AddBody(body);
        }
    }


    private static void AddStaticCollisionBodiesToOutline(TileMap tileMap, PhysicsWorld world)
    {

    }
}
