using SFML.Window;
using SFML.Graphics;

using Latte.Application;


namespace Milkway;




public static class Engine
{
    // TODO: move this to Latte
    // TODO:
    // the engine's philosophy should be: manual drawing and updating handling

    public static void InitFullScreen(string title, AppInitializationSettings? settings = null)
    {
        settings ??= AppInitializationSettings.Default;
        settings = settings.Value with { WindowStyle = Styles.Fullscreen };


        Init(VideoMode.DesktopMode, title, settings);
    }


    public static void Init(VideoMode mode, string title, AppInitializationSettings? settings = null)
    {
        App.Init(mode, title, settings);
    }
}
