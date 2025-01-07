
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Progressus.Soft.Maui.Components;

public partial class Alert : BorderItem, INotifyPropertyChanged
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
    public static readonly BindableProperty DismissibleProperty =
    BindableProperty.Create(
        propertyName: nameof(Dismissible),
        returnType: typeof(bool),
        declaringType: typeof(Alert),
        defaultValue: true);
    public bool Dismissible
    {
        get => (bool)GetValue(DismissibleProperty);
        set => SetValue(DismissibleProperty, value);
    }
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

	string _source;
	public string IconSource
	{
		get { return _source; }
		private set { SetProperty(ref _source, value); }
	}
	static void OnAlertTypeChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is Alert && newValue != null && newValue is AlertType)
        {
            switch((AlertType)newValue)
            {
                case AlertType.Success:
                    (bindable as Alert)!.Color = Color.Parse("#198754");
                    (bindable as Alert)!.IconSource = "ic_check_circle_white_48dp.png" ;
                    break;
                case AlertType.Danger:
                    (bindable as Alert)!.Color = Color.Parse("#dc3545");
					(bindable as Alert)!.IconSource = "ic_error_outline_white_48dp.png";
					break;
                case AlertType.Warning:
                    (bindable as Alert)!.Color = Color.Parse("#ffc107");
					(bindable as Alert)!.IconSource = "ic_warning_white_48dp.png";
					break;
                case AlertType.Information:
                    (bindable as Alert)!.Color = Color.Parse("#0dcaf0");
					(bindable as Alert)!.IconSource = "ic_info_outline_white_48dp.png";
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

    private async void CloseButton_Clicked(object sender, EventArgs e)
    {
        if((sender as ImageButton).Command == null)
        {
            var parent = Parent;
            //Find out if alert is a child of a modal content page (displayed as modal)
            if(parent is not null && parent is ContentPage && (parent as ContentPage)!.Navigation.ModalStack.Any(l => l.Id == parent.Id))
            {
				await (parent as ContentPage)!.Navigation.PopModalAsync(false);
			}else
                IsVisible = false;
        }
    }

	/// <summary>
	/// Display alert as a modal window
	/// </summary>
	/// <param name="navigation">Navigation Stack</param>
	/// <param name="title">Alert Title</param>
	/// <param name="message">Alert Message</param>
	/// <param name="alertType">Alert Type</param>
	/// <param name="verticalOptions">Vertical Options</param>
	/// <param name="horizontalOptions">Horizontal Options</param>
	/// <param name="overlayColor">Alert Background color</param>
	/// <param name="layout">Custom content page to show alert inside</param>
	/// <returns>A task representing the current operation</returns>
	/// <exception cref="ArgumentNullException"></exception>
	public static async Task DisplayAlertAsync(INavigation navigation, string title, string message, 
        AlertType alertType = AlertType.Success, 
        LayoutOptions? verticalOptions = null,
        LayoutOptions? horizontalOptions = null,
        Color? overlayColor = null,
		ContentPage? layout = null)
    {
        if(navigation is null) throw new ArgumentNullException(nameof(navigation));

        //Configure layout
		ContentPage container = layout ?? new ();
        container.BackgroundColor = overlayColor ?? Color.Parse("Transparent");
        
        Alert alert = new()
		{
			Title = title,
			Message = message,
			AlertType = alertType,
			VerticalOptions = verticalOptions ?? LayoutOptions.Center,
			HorizontalOptions = horizontalOptions ?? LayoutOptions.Center,
        };
        container.Content = alert;
		await navigation.PushModalAsync(container, false);
	}

    /// <summary>
    /// Display alert as a modal window
    /// </summary>
    /// <param name="navigation">Navigation Stack</param>
    /// <param name="instance">Alert instance</param>
    /// <param name="overlayColor">Alert Background color</param>
    /// <param name="layout">Custom content page to show alert inside</param>
    /// <returns>A task representing the current operation</returns>
    /// <exception cref="ArgumentNullException"></exception>
	public static async Task DisplayAlertAsync(INavigation navigation, Alert instance,
		Color? overlayColor = null,
		ContentPage? layout = null)
	{
		if (navigation is null) throw new ArgumentNullException(nameof(navigation));
		if (instance is null) throw new ArgumentNullException(nameof(instance));

		//Configure layout
		ContentPage container = layout ?? new();
		container.BackgroundColor = overlayColor ?? Color.Parse("Transparent");
        container.Content = instance;
		await navigation.PushModalAsync(container, false);
	}
}

public enum AlertType
{
    Success,
    Warning,
    Danger,
    Information
}