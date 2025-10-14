using SFML.Graphics;

using Latte.Core.Objects;


namespace Milkway.Tiles;




public class Tile : BaseObject
{
    public override Transformable SfmlTransformable => Sprite.SfmlTransformable;
    public override Drawable SfmlDrawable => Sprite.SfmlDrawable;




    public uint Size { get; set; }

    public Sprite Sprite { get; set; }
    public bool Empty { get; set; }




    // TODO: tile must use tile set id system obligatorily, unless you find a better way

    public Tile(uint size)
    {
        Size = size;
        Sprite = TileSet.GetEmptyTileTextureOfSize(size);
        Empty = true;
    }


    public Tile(Sprite sprite)
    {
        Size = sprite.SfmlSprite.Texture.Size.X; // supposing the sprite is symmetric
        Sprite = sprite;
        Empty = false;
    }




    public override void UnconditionalUpdate()
    {
        Sprite.Position = Position;

        base.UnconditionalUpdate();
    }




    public override FloatRect GetBounds()
        => Sprite.GetBounds();
}
