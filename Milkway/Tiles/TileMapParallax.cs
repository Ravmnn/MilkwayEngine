using System.Linq;

using SFML.Graphics;

using DotTiled;


namespace Milkway.Tiles;


public class TileMapParallax : Parallax
{
    public TileMapParallax(Camera camera, TileSet tileSet, Map map, IntRect? area = null, IParallaxCalculator? calculator = null)
        : base(camera, calculator)
    {
        InitializeFromTiledTileMap(tileSet, map, area);
    }


    private void InitializeFromTiledTileMap(TileSet tileSet, Map map, IntRect? area = null)
    {
        var tileMaps = TileMap.GetTileMapsFromTiledTileMap(tileSet, map, area).ToArray();
        var depth = tileMaps.Length - 1;

        foreach (var tileMap in tileMaps)
            Layers.Add(new TileMapParallaxLayer(tileMap, depth--));
    }


    public void AddTilesToApp()
    {
        foreach (var layer in Layers)
            if (layer is TileMapParallaxLayer tileLayer)
                tileLayer.TileMap.AddTilesToApp();
    }

    public void RemoveTilesFromApp()
    {
        foreach (var layer in Layers)
            if (layer is TileMapParallaxLayer tileLayer)
                tileLayer.TileMap.RemoveTilesFromApp();
    }
}
