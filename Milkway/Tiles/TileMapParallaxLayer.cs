using SFML.Graphics;

using Latte.Core;


using SfSprite = SFML.Graphics.Sprite;


namespace Milkway.Tiles;


public class TileMapParallaxLayer : ParallaxLayer
{
    private bool _shouldDrawTileMapToContents;


    public RenderTexture RenderTexture { get; set; }
    public TextureRenderer Renderer { get; set; }

    public TileMap TileMap { get; set; }


    public TileMapParallaxLayer(TileMap tileMap, float depth, int relativePriority = 0)
        : base(new SfSprite(), depth, relativePriority)
    {
        _shouldDrawTileMapToContents = true;


        RenderTexture = new RenderTexture(tileMap.WidthInPixels, tileMap.HeightInPixels);
        Renderer = new TextureRenderer(RenderTexture);

        TileMap = tileMap;
    }


    public override void Update()
    {
        Content = Renderer.RenderTextureSprite;

        base.Update();
    }


    public override void Draw(IRenderer target)
    {
        if (_shouldDrawTileMapToContents)
        {
            TileMap.Draw(Renderer);
            _shouldDrawTileMapToContents = false;
        }

        var oldPosition = Content.Position;
        FixScale();

        base.Draw(target);

        Content.Position = oldPosition;
    }


    private void FixScale()
    {
        // looks like RenderTexture store the texture in OpenGL's coordinate system, so it's necessary
        // to flip the texture to get the right appearance

        Content.Scale = Content.Scale with { Y = -Content.Scale.Y };
        Content.Position = Content.Position with { Y = Content.Position.Y + Content.GetGlobalBounds().Height };
    }
}
