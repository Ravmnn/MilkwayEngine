using System.Collections.Generic;

using SFML.Graphics;

using Latte.Core.Type;

using Milkway.Exceptions.Tiles;


namespace Milkway.Tiles;




public readonly record struct TileSetItem(Texture Texture, IntRect Area, uint Id);


public class TileSet
{
    public const int EmptyId = 0;




    public Image Image { get; }
    public Texture ImageTexture { get; }

    public List<TileSetItem> TileCache { get; }


    public uint TileSize { get; }
    public uint TileCount => GetTileCountOrThrow();




    public TileSet(Image image, uint tileSize)
    {
        Image = image;
        ImageTexture = new Texture(Image);

        TileCache = [];
        TileSize = tileSize;

        // empty tile
        TileCache.Add(new TileSetItem(GetEmptyTextureOfSize(tileSize), new IntRect(), EmptyId));

        if (Image.Size.X != Image.Size.Y)
            throw new AssymetricTileSetSizeException();
    }




    public static Texture GetEmptyTextureOfSize(uint size)
        => ColorTexture.FromColor(size, size, Color.Transparent);




    private uint GetTileCountOrThrow()
    {
        var rest = Image.Size.X % TileSize;
        var tileCount = Image.Size.X / TileSize;

        if (rest != 0)
            throw new InvalidTileSetSizeException();

        return tileCount * tileCount;
    }




    public TileSetItem GetTile(uint id)
    {
        var cacheTile = GetTileFromCache(id);

        var area = GetTileArea(id)!.Value;
        var tile = cacheTile ?? new TileSetItem(new Texture(Image, area), area, id);

        if (cacheTile is null)
            TileCache.Add(tile);

        return tile;
    }


    private TileSetItem? GetTileFromCache(uint id)
    {
        foreach (var tile in TileCache)
            if (tile.Id == id)
                return tile; // texture sharing the same memory address

        return null;
    }




    public IntRect? GetTileArea(uint id)
    {
        // TODO: algorithm probably can be improved

        if (id == EmptyId)
            return new IntRect();

        var position = new Vec2i();
        var size = new Vec2i((int)TileSize, (int)TileSize);

        for (var i = 1; i <= TileCount; i++)
        {
            if (i == id)
                return new IntRect(position, size);

            position.X += (int)TileSize;

            if (position.X < Image.Size.X)
                continue;

            position.X = 0;
            position.Y += (int)TileSize;
        }

        return null;
    }
}
