namespace Progressus.Soft.Maui.Components;

public partial class BorderedImage : BorderItem
{
    public static readonly BindableProperty ImageSourceProperty =
    BindableProperty.Create(
        propertyName: nameof(ImageSource),
        returnType: typeof(ImageSource),
        declaringType: typeof(BorderedImage),
        defaultValue: null);

    public static readonly BindableProperty AspectProperty =
    BindableProperty.Create(
        propertyName: nameof(Aspect),
        returnType: typeof(Aspect),
        declaringType: typeof(BorderedImage),
        defaultValue: Aspect.AspectFit);

    public ImageSource ImageSource
    {
        get => (ImageSource)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
    public Aspect Aspect
    {
        get => (Aspect)GetValue(AspectProperty);
        set => SetValue(AspectProperty, value);
    }
    public BorderedImage()
	{
        InitializeComponent();
	}
}