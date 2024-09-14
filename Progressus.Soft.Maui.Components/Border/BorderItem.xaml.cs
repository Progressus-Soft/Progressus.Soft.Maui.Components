namespace Progressus.Soft.Maui.Components;

public partial class BorderItem : Border
{
    public static readonly BindableProperty ShapeCornerRadiusProperty =
    BindableProperty.Create(
        propertyName: nameof(ShapeCornerRadius),
        returnType: typeof(CornerRadius),
        declaringType: typeof(BorderItem));
    public CornerRadius ShapeCornerRadius
    {
        get => (CornerRadius)GetValue(ShapeCornerRadiusProperty);
        set => SetValue(ShapeCornerRadiusProperty, value);
    }
    public BorderItem()
	{
        InitializeComponent();
	}
}