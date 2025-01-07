namespace Progressus.Soft.Maui.Components;

public partial class BorderedActivityIndicator : BorderItem
{
    public static readonly BindableProperty IsRunningProperty =
    BindableProperty.Create(
        propertyName: nameof(IsRunning),
        returnType: typeof(bool),
        declaringType: typeof(BorderedActivityIndicator),
        defaultValue: false);

    public bool IsRunning
    {
        get => (bool)GetValue(IsRunningProperty);
        set => SetValue(IsRunningProperty, value);
    }
    public BorderedActivityIndicator()
	{
		InitializeComponent();
	}
}