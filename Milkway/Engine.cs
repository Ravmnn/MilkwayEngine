using SFML.Window;
using SFML.Graphics;

using Latte.Application;


namespace Milkway;


public static class Engine
{
    public static void InitFullScreen(string title, Font? defaultFont = null)
        => Init(VideoMode.DesktopMode, title, defaultFont, Styles.Fullscreen);


    public static void Init(VideoMode mode, string title, Font? defaultFont = null,
        Styles style = Styles.Default, ContextSettings? contextSettings = null)
    {
        App.Init(mode, title,defaultFont, style, contextSettings);
        App.ManualClearDisplayProcess = true;
    }
}
