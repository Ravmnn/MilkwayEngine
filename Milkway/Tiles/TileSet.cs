using SFML.Graphics;

using Latte.Core.Type;

using Milkway.Exceptions.Tiles;


namespace Milkway.Tiles;


public class TileSet
{
    public Image Image { get; }

    public uint TileSize { get; }

    public uint TileCount => GetTileCountOrThrow();


    public TileSet(Image image, uint tileSize)
    {
        Image = image;
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

        return new Texture(Image, GetAreaOfTileByIndex(index)!.Value);
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
