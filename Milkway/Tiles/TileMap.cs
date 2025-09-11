using System;
using System.Diagnostics;
using System.Linq;

using SFML.Graphics;

using Latte.Core;
using Latte.Core.Type;
using Latte.Application;

using DotTiled;


using Color = SFML.Graphics.Color;


namespace Milkway.Tiles;


public enum TileCreationMode
{
    Share,
    Copy
}


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

        var stopwatch = Stopwatch.StartNew();
            InitializeTiles(startPosition ?? new Vec2f());
        stopwatch.Stop();

        Console.WriteLine($"Initializing tiles took {stopwatch.ElapsedMilliseconds}ms");
    }


    // TODO: Tiled uses layers, add support for them too
    // TODO: add parallax support

    public TileMap(TileSet tileSet, Map tileMap, IntRect? area = null, TileCreationMode creationMode = TileCreationMode.Share, Vec2f? startPosition = null)
        : this((uint?)area?.Width ?? tileMap.Width, (uint?)area?.Height ?? tileMap.Height, tileMap.TileWidth, startPosition)
    {
        var stopwatch = Stopwatch.StartNew();
            LoadFromTiledTileMap(tileSet, tileMap, area, creationMode);
        stopwatch.Stop();

        Console.WriteLine($"Initializing tiles from tile map took {stopwatch.ElapsedMilliseconds}ms");
    }


    public void AddTilesToApp()
        => App.AddObjects(Tiles.Cast<BaseObject>());

    public void RemoveTilesFromApp()
        => App.RemoveObjects(Tiles.Cast<BaseObject>());


    private void InitializeTiles(Vec2f startPosition)
    {
        var currentPosition = startPosition.Copy();
        var emptySprite = ColorTexture.FromColor(8, 8, Color.Transparent);

        for (var y = 0u; y < Height; y++, currentPosition.Y += TileSize)
        {
            for (var x = 0u; x < Width; x++, currentPosition.X += TileSize)
                Tiles[y, x] = new Tile(emptySprite) // using the same memory address for the empty sprite for all tiles.
                {
                    Position = currentPosition.Copy()
                };

            currentPosition.X = startPosition.X;
        }
    }


    private void LoadFromTiledTileMap(TileSet tileSet, Map tileMap, IntRect? area = null, TileCreationMode creationMode = TileCreationMode.Share)
    {
        if (tileMap.Layers[0] is not TileLayer tileLayer)
            return;

        area ??= new IntRect(0, 0, (int)Width, (int)Height);

        var tileIds = TileIdArrayToMatrix(tileLayer.Data.Value.GlobalTileIDs.Value, tileLayer.Width, tileLayer.Height);

        for (var y = 0; y < area.Value.Height; y++)
        for (var x = 0; x < area.Value.Width; x++)
        {
            var indexY = y + area.Value.Top;
            var indexX = x + area.Value.Left;

            var id = tileIds[indexY, indexX];

            var texture = tileSet.GetTileTextureByIndex(id);
            texture = creationMode == TileCreationMode.Copy ? new Texture(texture) : texture;

            Tiles[y, x].Sprite = texture;
        }
    }


    private static uint[,] TileIdArrayToMatrix(uint[] array, uint width, uint height)
    {
        var matrix = new uint[height, width];

        for (var y = 0; y < height; y++)
            for (var x = 0; x < width; x++)
                matrix[y, x] = array[y * width + x];

        return matrix;
    }
}
