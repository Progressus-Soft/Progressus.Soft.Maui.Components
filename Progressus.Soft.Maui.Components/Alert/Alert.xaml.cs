
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Progressus.Soft.Maui.Components;

public partial class Alert : ContentView, INotifyPropertyChanged
{
    public static readonly BindableProperty TitleProperty =
    BindableProperty.Create(
        propertyName: nameof(Title),
        returnType: typeof(string),
        declaringType: typeof(Alert),
        defaultValue: string.Empty);
    public static readonly BindableProperty MessageProperty =
    BindableProperty.Create(
        propertyName: nameof(Message),
        returnType: typeof(string),
        declaringType: typeof(Alert),
        defaultValue: string.Empty);
    public static readonly BindableProperty ClosingCommandProperty =
    BindableProperty.Create(
        propertyName: nameof(ClosingCommand),
        returnType: typeof(Command),
        declaringType: typeof(Alert),
        defaultValue: null);
    public static readonly BindableProperty RefreshCommandProperty =
    BindableProperty.Create(
        propertyName: nameof(RefreshCommand),
        returnType: typeof(Command),
        declaringType: typeof(Alert),
        defaultValue: null);
    public static readonly BindableProperty DisplayRefreshButtonProperty =
    BindableProperty.Create(
        propertyName: nameof(DisplayRefreshButton),
        returnType: typeof(bool),
        declaringType: typeof(Alert),
        defaultValue: false);
    public static readonly BindableProperty DisplayCloseButtonProperty =
    BindableProperty.Create(
        propertyName: nameof(DisplayCloseButton),
        returnType: typeof(bool),
        declaringType: typeof(Alert),
        defaultValue: true);
    public static readonly BindableProperty AlertTypeProperty =
    BindableProperty.Create(
        propertyName: nameof(AlertType),
        returnType: typeof(AlertType),
        declaringType: typeof(Alert),
        defaultValue: AlertType.Danger,
        propertyChanged: OnAlertTypeChanged);
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public Command ClosingCommand
    {
        get => (Command)GetValue(ClosingCommandProperty);
        set => SetValue(ClosingCommandProperty, value);
    }
    public Command RefreshCommand
    {
        get => (Command)GetValue(RefreshCommandProperty);
        set => SetValue(RefreshCommandProperty, value);
    }
    public bool DisplayRefreshButton
    {
        get => (bool)GetValue(DisplayRefreshButtonProperty);
        set => SetValue(DisplayRefreshButtonProperty, value);
    }
    public bool DisplayCloseButton
    {
        get => (bool)GetValue(DisplayCloseButtonProperty);
        set => SetValue(DisplayCloseButtonProperty, value);
    }

    public AlertType AlertType
    {
        get => (AlertType)GetValue(AlertTypeProperty);
        set
        {
            SetValue(AlertTypeProperty, value);
            OnAlertTypeChanged(this, null, value);
        }
    }

    Color _color = Color.Parse("#dc3545");
    public Color Color
    {
        get { return _color; }
        private set { SetProperty(ref _color, value); }
    }
    static void OnAlertTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is Alert && newValue != null && newValue is AlertType)
        {
            switch((AlertType)newValue)
            {
                case AlertType.Success:
                    (bindable as Alert)!.Color = Color.Parse("#198754");
                    break;
                case AlertType.Danger:
                    (bindable as Alert)!.Color = Color.Parse("#dc3545");
                    break;
                case AlertType.Warning:
                    (bindable as Alert)!.Color = Color.Parse("#ffc107");
                    break;
                case AlertType.Information:
                    (bindable as Alert)!.Color = Color.Parse("#0dcaf0");
                    break;
            }
        }
    }

    protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
    {
        if (EqualityComparer<T>.Default.Equals(backingStore, value))
            return false;

        backingStore = value;
        onChanged?.Invoke();
        OnPropertyChanged(propertyName);
        return true;
    }
    public Alert()
	{
		InitializeComponent();
	}
    public Alert(string title, string message, bool displayRefresh = false, AlertType alertType = AlertType.Success)
    {
        InitializeComponent();
        Title = title;
        Message = message;
        DisplayRefreshButton = displayRefresh;
    }

    private void CloseButton_Clicked(object sender, EventArgs e)
    {
        if((sender as Button).Command == null)
        {
            IsVisible = false;
        }
    }
}

public enum AlertType
{
    Success,
    Warning,
    Danger,
    Information
}