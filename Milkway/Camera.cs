using SFML.Graphics;


namespace Milkway;


public class Camera(RenderTarget target)
{
    private View _view = target.GetView();


    public RenderTarget Target { get; } = target;

    public View View
    {
        get => _view;
        set
        {
            _view = value;
            Target.SetView(_view);
        }
    }
}
