using SFML.Graphics;

using Latte.Core;


namespace Milkway.Tiles;


public class Tile : BaseObject
{
    public override Transformable SfmlTransformable { get; }
    public override Drawable SfmlDrawable { get; }


    public override FloatRect GetBounds()
    {
        throw new System.NotImplementedException();
    }
}
