using Latte.Core.Type;


namespace PathTracer2D.Engine;




public class RaySource(Vec2f position, int rayCount)
{
    public Vec2f Position { get; set; } = position;
    public int RayCount { get; set; } = rayCount;




    public IEnumerable<Ray> GenerateRays()
    {
        var rays = new List<Ray>();

        for (var i = 0; i < RayCount; i++)
            rays.Add(new Ray(Position, RandomDirection()));

        return rays;
    }


    private static Vec2f RandomDirection()
    {
        var generator = new Random();
        var direction = new Vec2f(generator.NextSingle(), generator.NextSingle());

        return direction * 2 - new Vec2f(1, 1);
    }
}
