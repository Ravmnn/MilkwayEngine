using DotTiled.Serialization;

using Latte.Core;


namespace Milkway.Tiles.Tiled;


public class TiledEmbeddedResourceReader(string location) : IResourceReader
{
    public string Location { get; } = location;


    public string Read(string resourceName)
        => EmbeddedResourceLoader.LoadText($"{Location}.{resourceName}");
}
