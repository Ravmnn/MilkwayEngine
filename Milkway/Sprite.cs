using SFML.Graphics;

using Latte.Core;


using SfSprite = SFML.Graphics.Sprite;


namespace Milkway;


public class Sprite : BaseObject
{
    public override Transformable SfmlTransformable => SfmlSprite;
    public override Drawable SfmlDrawable => SfmlSprite;

    public SfSprite SfmlSprite { get; }


    public Sprite(Texture texture)
    {
        SfmlSprite = new SfSprite(texture);
    }


    public override FloatRect GetBounds()
        => SfmlSprite.GetGlobalBounds();
}
