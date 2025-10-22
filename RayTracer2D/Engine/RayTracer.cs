using SFML.Graphics;

using Latte.Core.Type;



namespace RayTracer2D.Engine;




public readonly record struct PixelColor(Vec2f Position, ColorRGBA Color);




public class RayTracer()
{
    public List<Segment> Segments { get; set; } = [];
    public List<RaySource> RaySources { get; set; } = [];

    public List<Ray> Rays { get; set; } = [];




    public RayTracer(List<Segment> segments, List<RaySource> raySources) : this()
    {
        Segments = segments;
        RaySources = raySources;
    }




    public Image Render(Vec2u resolution)
    {
        var pixels = new Color[resolution.X, resolution.Y];
        var intersectionPoints = TraceAll();
        var pixelColors
            = from intersectionPoint in intersectionPoints select new PixelColor(intersectionPoint.Point, Color.White);

        foreach (var pixelColor in pixelColors)
        {
            var indexX = (int)Math.Round(pixelColor.Position.X);
            var indexY = (int)Math.Round(pixelColor.Position.Y);

            if (indexX < 0 || indexX >= pixels.GetLength(0) ||
                indexY < 0 || indexY >= pixels.GetLength(1))
                continue;

            pixels[indexX, indexY] = pixelColor.Color;
        }

        return new Image(pixels);
    }




    public IEnumerable<IntersectionPoint> TraceAll()
    {
        var intersectionPoints = new List<IntersectionPoint>();

        GenerateRaysFromSources();

        foreach (var ray in Rays)
            if (Trace(ray) is { } intersectionPoint)
                intersectionPoints.Add(intersectionPoint);

        return intersectionPoints;
    }


    private void GenerateRaysFromSources()
    {
        Rays.Clear();

        foreach (var raySource in RaySources)
            Rays.AddRange(raySource.GenerateRays());
    }




    private IntersectionPoint? Trace(Ray ray)
    {
        var intersectionPoints = new List<IntersectionPoint>();

        foreach (var segment in Segments)
            if (ray.IntersectsSegment(segment, out var t, out var u))
                intersectionPoints.Add(new IntersectionPoint(ray, t, u));

        if (intersectionPoints.Count == 0)
            return null;

        intersectionPoints = intersectionPoints.OrderBy(point => point.RayT).ToList();

        return intersectionPoints.First();
    }
}
