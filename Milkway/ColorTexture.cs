using SFML.Graphics;

using Latte.Core.Type;


namespace Milkway;


public static class ColorTexture
{
    public static Texture FromColor(uint width, uint height, ColorRGBA color)
    {
        var image = new Image(width, height);

        for (var x = 0u; x < width; x++)
            for (var y = 0u; y < height; y++)
                image.SetPixel(x, y, color);

        return new Texture(image);
    }
}
