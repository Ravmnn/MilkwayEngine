using SFML.Graphics;


namespace Milkway.Physics;


public static class Collision
{
    public static bool IsColliding(FloatRect a, FloatRect b, out FloatRect intersection)
        => a.Intersects(b, out intersection);
}
