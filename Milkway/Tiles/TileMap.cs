using DotTiled;

using Latte.Core.Type;


using Color = SFML.Graphics.Color;


namespace Milkway.Tiles;


public class TileMap
{
    public Tile[,] Tiles { get; }

    public uint Width { get; }
    public uint Height { get; }
    public uint TileSize { get; }


    public TileMap(uint width, uint height, uint tileSize, Vec2f? startPosition = null)
    {
        Width = width;
        Height = height;
        TileSize = tileSize;

        Tiles = new Tile[Height, Width];

        InitializeTiles(startPosition ?? new Vec2f());
    }


    // TODO: Tiled uses layers, add support for them too
    // TODO: add parallax support

    public TileMap(TileSet tileSet, Map tileMap, Vec2f? startPosition = null)
        : this(tileMap.Width, tileMap.Height, tileMap.TileWidth, startPosition)
    {
        LoadFromTiledTileMap(tileSet, tileMap);
    }


    private void InitializeTiles(Vec2f startPosition)
    {
        var currentPosition = startPosition.Copy();

        for (var y = 0u; y < Height; y++, currentPosition.Y += TileSize)
        {
            for (var x = 0u; x < Width; x++, currentPosition.X += TileSize)
                Tiles[y, x] = new Tile(ColorTexture.FromColor(8, 8, Color.Transparent))
                {
                    Position = currentPosition.Copy()
                };

            currentPosition.X = startPosition.X;
        }
    }


    private void LoadFromTiledTileMap(TileSet tileSet, Map tileMap)
    {
        if (tileMap.Layers[0] is not TileLayer tileLayer)
            return;

        var index = 0;

        for (var y = 0; y < Height; y++)
        for (var x = 0; x < Width; x++, index++)
        {
            var id = tileLayer.Data.Value.GlobalTileIDs.Value[index];

            Tiles[y, x].Sprite = tileSet.GetTileTextureByIndex(id);
        }
    }
}
