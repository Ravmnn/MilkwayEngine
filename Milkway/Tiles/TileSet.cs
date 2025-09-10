using System.Linq;
using System.Collections.Generic;

using SFML.Graphics;

using Latte.Core.Type;

using Milkway.Exceptions.Tiles;


namespace Milkway.Tiles;


public class TileSet
{
    public Image Image { get; }

    public List<(uint, Texture)> TileCache { get; private set; }

    public uint TileSize { get; }

    public uint TileCount => GetTileCountOrThrow();


    public TileSet(Image image, uint tileSize)
    {
        Image = image;
        TileCache = [];
        TileSize = tileSize;

        if (Image.Size.X != Image.Size.Y)
            throw new AssymetricTileSetSizeException();
    }


    private uint GetTileCountOrThrow()
    {
        var rest = Image.Size.X % TileSize;
        var tileCount = Image.Size.X / TileSize;

        if (rest != 0)
            throw new InvalidTileSetSizeException();

        return tileCount * tileCount;
    }


    public Texture GetTileTextureByIndex(uint index)
    {
        // tile index (id) of 0 means empty
        if (index == 0)
            return ColorTexture.FromColor(TileSize, TileSize, Color.Transparent);

        var cacheTexture = GetTileTextureFromCache(index);
        var texture = cacheTexture ?? new Texture(Image, GetAreaOfTileByIndex(index)!.Value);

        if (cacheTexture is null)
            TileCache.Add((index, texture));

        return texture;
    }


    private Texture? GetTileTextureFromCache(uint index)
    {
        // TODO: be able to choose whether or not to use the same sprite memory address
        foreach (var tile in TileCache)
            if (tile.Item1 == index)
                return tile.Item2; // sharing the same memory address! currently, individual objects are not needed

        return null;
    }


    private IntRect? GetAreaOfTileByIndex(uint index)
    {
        var position = new Vec2i();
        var size = new Vec2i((int)TileSize, (int)TileSize);

        for (var i = 1; i <= TileCount; i++)
        {
            if (i == index)
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
