using System;
using System.Collections.Generic;
using System.Diagnostics;

using SFML.Graphics;

using Latte.Core;
using Latte.Core.Type;
using Latte.Core.Objects;
using Latte.Rendering;

using DotTiled;


namespace Milkway.Tiles;




public class TileMap : IUpdateable, IDrawable
{
    protected VertexBuffer RenderBuffer { get; set; }
    protected Vertex[] Vertices { get; }

    protected bool ShouldUpdateVertices { get; set; }


    public TileSet TileSet { get; }
    public Tile[,] Tiles { get; }

    public uint Width { get; }
    public uint Height { get; }
    public uint TileSize { get; }

    public uint WidthInPixels => Width * TileSize;
    public uint HeightInPixels => Height * TileSize;


    public event EventHandler? UpdateEvent;
    public event EventHandler? DrawEvent;




    private TileMap(uint width, uint height, uint tileSize, TileSet tileSet)
    {
        var vertexCount = width * height * 4;
        RenderBuffer = new VertexBuffer(vertexCount, PrimitiveType.Quads, VertexBuffer.UsageSpecifier.Static);
        Vertices = new Vertex[vertexCount];

        ShouldUpdateVertices = true;


        Width = width;
        Height = height;
        TileSize = tileSize;
        TileSet = tileSet;

        Tiles = new Tile[Height, Width];
    }


    public TileMap(TileSet tileSet, TileLayer tileLayer, IntRect? area = null)
        : this((uint?)area?.Width ?? tileLayer.Width, (uint?)area?.Height ?? tileLayer.Height, tileSet.TileSize, tileSet)
    {
        var stopwatch = Stopwatch.StartNew();
            LoadFromTiledTileLayer(tileSet, tileLayer, area);
        stopwatch.Stop();

        Console.WriteLine($"Initializing tiles from tile map took {stopwatch.ElapsedMilliseconds}ms");
    }


    private void LoadFromTiledTileLayer(TileSet tileSet, TileLayer layer, IntRect? area = null)
    {
        area ??= new IntRect(0, 0, (int)Width, (int)Height);

        var tileIds = TileIdArrayToMatrix(layer.Data.Value.GlobalTileIDs.Value, layer.Width, layer.Height);

        for (var y = 0u; y < area.Value.Height; y++)
        for (var x = 0u; x < area.Value.Width; x++)
        {
            var indexY = y + area.Value.Top;
            var indexX = x + area.Value.Left;

            var id = tileIds[indexY, indexX];
            ref var tile = ref Tiles[y, x];

            tile = new Tile(tileSet, id)
            {
                Position = GetPositionOfTileAt(x, y)
            };
        }
    }


    private Vec2f GetPositionOfTileAt(uint x, uint y)
        => new Vec2f(x * TileSize, y * TileSize);


    private static uint[,] TileIdArrayToMatrix(uint[] array, uint width, uint height)
    {
        var matrix = new uint[height, width];

        for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            matrix[y, x] = array[y * width + x];

        return matrix;
    }




    public virtual void Update()
    {
        foreach (var tile in Tiles)
            tile.UpdateObject();

        UpdateEvent?.Invoke(this, EventArgs.Empty);
    }




    public virtual void Draw(IRenderer renderer)
    {
        if (ShouldUpdateVertices)
        {
            UpdateVertices();
            RenderBuffer.Update(Vertices);
            ShouldUpdateVertices = false;
        }

        renderer.Render(RenderBuffer, texture: TileSet.ImageTexture);

        DrawEvent?.Invoke(this, EventArgs.Empty);
    }


    private void UpdateVertices()
    {
        for (var y = 0; y < Tiles.GetLength(0); y++)
        for (var x = 0; x < Tiles.GetLength(1); x++)
        {
            var tile = Tiles[y, x];
            var verticesIndex = (y * Width + x) * 4;

            var newVertices = VerticesOfTile(tile);

            Vertices[verticesIndex + 0] = newVertices[0];
            Vertices[verticesIndex + 1] = newVertices[1];
            Vertices[verticesIndex + 2] = newVertices[2];
            Vertices[verticesIndex + 3] = newVertices[3];
        }
    }


    private Vertex[] VerticesOfTile(Tile tile)
    {
        var vertices = new Vertex[4];

        var tileVertices = tile.GetBounds().RectToVertices();
        var textureVertices = ((FloatRect)tile.Source.Area).RectToVertices();

        if (tile.Empty || !tile.CanDraw)
        {
            vertices[0] = new Vertex();
            vertices[1] = new Vertex();
            vertices[2] = new Vertex();
            vertices[3] = new Vertex();
        }
        else
        {
            vertices[0] = new Vertex(tileVertices.TopLeft, textureVertices.TopLeft);
            vertices[1] = new Vertex(tileVertices.TopRight, textureVertices.TopRight);
            vertices[2] = new Vertex(tileVertices.BottomRight, textureVertices.BottomRight);
            vertices[3] = new Vertex(tileVertices.BottomLeft, textureVertices.BottomLeft);
        }

        return vertices;
    }




    public Tile? TryGet(uint x, uint y)
    {
        if (x >= Width || y >= Height)
            return null;

        return Tiles[y, x];
    }




    public static IEnumerable<TileMap> GetTileMapsFromTiledTileMap(TileSet tileSet, Map map, IntRect? area = null)
    {
        var tileMaps = new List<TileMap>();

        foreach (var layer in map.Layers)
            if (layer is TileLayer tileLayer)
                tileMaps.Add(new TileMap(tileSet, tileLayer, area));

        return tileMaps;
    }
}
