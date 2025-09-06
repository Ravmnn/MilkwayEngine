using SFML.Graphics;

using Latte.Core.Type;


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
}
