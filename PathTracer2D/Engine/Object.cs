namespace PathTracer2D.Engine;




public abstract class Object
{
    public Segment[] Segments { get; protected set; }
    public Material Material { get; set; }
}
