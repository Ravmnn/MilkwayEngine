using System.Reflection;

using Latte.Core;
using Latte.Application;


namespace RayTracer2D;




class Program
{
    private static void Main()
    {
        var settings = AppInitializationSettings.Default;
        var contextSettings = AppInitializationSettings.DefaultContextSettings with { AntialiasingLevel = 0 };

        settings = settings with { ContextSettings = contextSettings };


        Milkway.Engine.InitFullScreen("2D Ray Tracer", settings);

        EmbeddedResourceLoader.ResourcesPath = "RayTracer2D.Resources";
        EmbeddedResourceLoader.SourceAssembly = Assembly.GetExecutingAssembly();


        App.Section = new MainSection();


        while (!App.ShouldQuit)
        {
            App.Update();
            App.Draw();
        }


        App.Deinit();
    }
}
