using SFML.Graphics;

using Latte.Core;


namespace Milkway.Tiles;


public class Tile : BaseObject
{
    public override Transformable SfmlTransformable => Sprite.SfmlTransformable;
    public override Drawable SfmlDrawable => Sprite.SfmlDrawable;

    public Sprite Sprite { get; set; }


    public Tile(Sprite sprite)
    {
        Sprite = sprite;
    }


    public override void Update()
    {
        Sprite.Position = Position;
    }


    public override FloatRect GetBounds()
        => Sprite.GetBounds();
}
