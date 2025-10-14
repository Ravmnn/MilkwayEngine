using Milkway.Physics;


namespace Milkway.Tiles;




public enum TileMapCollisionMethod
{
    All,
    Outline
}




public static class TileMapCollisionExtensions
{
    public static void ToSolidTiles(this TileMap tileMap, PhysicsWorld world, TileMapCollisionMethod collisionMethod)
    {
        switch (collisionMethod)
        {
            case TileMapCollisionMethod.All:
                AllToSolidTiles(tileMap, world);
                break;

            case TileMapCollisionMethod.Outline:
                OutlineToSolidTiles(tileMap, world);
                break;
        }
    }




    private static void AllToSolidTiles(TileMap tileMap, PhysicsWorld world)
    {
        for (var y = 0; y < tileMap.Tiles.GetLength(0); y++)
        for (var x = 0; x < tileMap.Tiles.GetLength(1); x++)
        {
            ref var tile = ref tileMap.Tiles[y, x];

            if (!tile.Empty)
                tile = new SolidTile(world, tile);
        }
    }




    private static void OutlineToSolidTiles(TileMap tileMap, PhysicsWorld world)
    {
        for (var y = 0; y < tileMap.Tiles.GetLength(0); y++)
        for (var x = 0; x < tileMap.Tiles.GetLength(1); x++)
        {
            ref var tile = ref tileMap.Tiles[y, x];

            if (IsTileAtOutline(tileMap, y, x))
                tile = new SolidTile(world, tile);
        }
    }


    private static bool IsTileAtOutline(TileMap tileMap, int x, int y)
    {
        if (tileMap.TryGet((uint)y, (uint)x)?.Empty ?? true)
            return false;

        for (var yb = y - 1; yb <= y + 1; yb++)
        for (var xb = x - 1; xb <= x + 1; xb++)
        {
            var isOriginTile = xb == x && yb == y;
            var isAtCorner = xb != x && yb != y;

            if (isOriginTile || isAtCorner)
                continue;

            var tile = tileMap.TryGet((uint)yb, (uint)xb);

            if (tile is null || tile.Empty)
                return true;
        }

        return false;
    }
}
