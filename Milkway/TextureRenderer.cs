using SFML.Graphics;

using Latte.Application;


using SfSprite = SFML.Graphics.Sprite;


namespace Milkway;


// TODO: move this to Latte


public class TextureRenderer : DefaultRenderer
{
    public RenderTexture RenderTexture => (RenderTarget as RenderTexture)!;
    public SfSprite RenderTextureSprite { get; }


    public TextureRenderer(RenderTexture renderTexture) : base(renderTexture)
    {
        RenderTextureSprite = new SfSprite(RenderTexture.Texture);
    }
}
