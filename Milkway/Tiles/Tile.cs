using SFML.Graphics;

using Latte.Core.Objects;


namespace Milkway.Tiles;




public class Tile : BaseObject
{
    public override Transformable SfmlTransformable => Sprite.SfmlTransformable;
    public override Drawable SfmlDrawable => Sprite.SfmlDrawable;



    public TileSet TileSet { get; }
    public TileSetItem Source { get; }

    public Sprite Sprite { get; }  // TODO: can't Tile inherit directly from Sprite?
    public uint Size => TileSet.TileSize;

    public bool Empty => Source.Id == TileSet.EmptyId;




    public Tile(TileSet tileSet, uint id)
    {
        var tile = tileSet.GetTile(id);

        TileSet = tileSet;
        Source = tile;

        Sprite = new Sprite(tile.Texture);
    }




    public override void UnconditionalUpdate()
    {
        Sprite.Position = Position;

        base.UnconditionalUpdate();
    }




    public override FloatRect GetBounds()
        => Sprite.GetBounds();
}
