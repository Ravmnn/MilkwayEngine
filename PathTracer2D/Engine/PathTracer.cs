using SFML.Graphics;

using Latte.Core.Type;



namespace RayTracer2D.Engine;




public readonly record struct PixelColor(Vec2f Position, ColorRGBA Color);




public class PathTracer()
{
    public List<Segment> Segments { get; set; } = [];
    public List<RaySource> RaySources { get; set; } = [];

    public List<Ray> Rays { get; set; } = [];




    public PathTracer(List<Segment> segments, List<RaySource> raySources) : this()
    {
        Segments = segments;
        RaySources = raySources;
    }




    public Image Render(Vec2u resolution, Vec2u viewport)
    {
        var pixels = new Color[resolution.X, resolution.Y];
        var intersectionPoints = TraceAll();
        var pixelColors
            = from intersectionPoint in intersectionPoints select new PixelColor(intersectionPoint.Point, Color.White);

        foreach (var pixelColor in pixelColors)
            RenderPixel(pixels, pixelColor, resolution, viewport);

        return new Image(pixels);
    }


    private void RenderPixel(Color[,] pixels, PixelColor pixelColor, Vec2u resolution, Vec2u viewport)
    {
        var roundedPosition = new Vec2f(MathF.Round(pixelColor.Position.X), MathF.Round(pixelColor.Position.Y));
        var normalizedDeviceCoordinate = MapToNormalizedDeviceCoordinate(viewport, roundedPosition);
        var imagePixel = MapNormalizedDeviceCoordinateToPixel(resolution, normalizedDeviceCoordinate);

        if (imagePixel.X < 0 || imagePixel.X >= pixels.GetLength(0) ||
            imagePixel.Y < 0 || imagePixel.Y >= pixels.GetLength(1))
            return;

        pixels[imagePixel.X, imagePixel.Y] = pixelColor.Color;
    }


    private Vec2i MapNormalizedDeviceCoordinateToPixel(Vec2u imageResolution, Vec2f coordinate)
    {
        var unsignedCoordinate = (coordinate + new Vec2f(1, 1)) / 2;
        return imageResolution * unsignedCoordinate;
    }


    private Vec2f MapToNormalizedDeviceCoordinate(Vec2u viewport, Vec2f coordinate)
        => coordinate / (Vec2f)viewport * 2 - new Vec2f(1, 1);




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
