using SfSprite = SFML.Graphics.Sprite;


namespace Milkway;


public record ParallaxLayer(SfSprite Content, float Depth, int RelativePriority = 0);
