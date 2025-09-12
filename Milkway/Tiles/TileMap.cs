using System;
using System.Collections.Generic;
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


public class TileMap : IUpdateable, IDrawable
{
    public Tile[,] Tiles { get; }

    public uint Width { get; }
    public uint Height { get; }
    public uint TileSize { get; }

    public uint WidthInPixels => Width * TileSize;
    public uint HeightInPixels => Height * TileSize;

    public event EventHandler? UpdateEvent;
    public event EventHandler? DrawEvent;


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


    public TileMap(TileSet tileSet, Map tileMap, TileLayer tileLayer, IntRect? area = null, TileCreationMode creationMode = TileCreationMode.Share, Vec2f? startPosition = null)
        : this((uint?)area?.Width ?? tileMap.Width, (uint?)area?.Height ?? tileMap.Height, tileMap.TileWidth, startPosition)
    {
        var stopwatch = Stopwatch.StartNew();
            LoadFromTiledTileLayer(tileSet, tileLayer, area, creationMode);
        stopwatch.Stop();

        Console.WriteLine($"Initializing tiles from tile map took {stopwatch.ElapsedMilliseconds}ms");
    }


    public virtual void Update()
    {
        foreach (var tile in Tiles)
            App.UpdateObject(tile);

        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }


    public virtual void Draw(IRenderer target)
    {
        foreach (var tile in Tiles)
            App.DrawObject(target, tile);

        DrawEvent?.Invoke(this, EventArgs.Empty);
    }


    public void AddTilesToApp()
        => App.AddObjects(Tiles.Cast<BaseObject>());

    public void RemoveTilesFromApp()
        => App.RemoveObjects(Tiles.Cast<BaseObject>());


    private void InitializeTiles(Vec2f startPosition)
    {
        var currentPosition = startPosition.Copy();
        var emptySprite = ColorTexture.FromColor(TileSize, TileSize, Color.Transparent);

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


    private void LoadFromTiledTileLayer(TileSet tileSet, TileLayer layer, IntRect? area = null, TileCreationMode creationMode = TileCreationMode.Share)
    {
        area ??= new IntRect(0, 0, (int)Width, (int)Height);

        var tileIds = TileIdArrayToMatrix(layer.Data.Value.GlobalTileIDs.Value, layer.Width, layer.Height);

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


    public static IEnumerable<TileMap> GetTileMapsFromTiledTileMap(TileSet tileSet, Map map, IntRect? area = null)
    {
        var tileMaps = new List<TileMap>();

        foreach (var layer in map.Layers)
            if (layer is TileLayer tileLayer)
                tileMaps.Add(new TileMap(tileSet, map, tileLayer, area));

        return tileMaps;
    }
}
