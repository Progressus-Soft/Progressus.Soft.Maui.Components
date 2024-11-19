using Microsoft.Maui.Graphics.Text;
using Progressus.Soft.Maui.Components.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Progressus.Soft.Maui.Components;

public partial class Form : ContentView, INotifyPropertyChanged
{
    public static readonly BindableProperty SourceTypeProperty =
    BindableProperty.Create(
        propertyName: nameof(SourceType),
        returnType: typeof(Type),
        declaringType: typeof(Form));

    public static readonly BindableProperty ButtonTextProperty =
    BindableProperty.Create(
        propertyName: nameof(ButtonText),
        returnType: typeof(string),
        declaringType: typeof(Form),
        propertyChanged: ButtonTextChanged);

    public static readonly BindableProperty DisplayLabelsProperty =
    BindableProperty.Create(
        propertyName: nameof(DisplayLabels),
        defaultValue: true,
        returnType: typeof(bool),
        declaringType: typeof(Form));

    public static readonly BindableProperty CommandProperty =
    BindableProperty.Create(
        propertyName: nameof(Command),
        returnType: typeof(ICommand),
        declaringType: typeof(Form));

    public static readonly BindableProperty CommandParameterProperty =
    BindableProperty.Create(
        propertyName: nameof(CommandParameter),
        returnType: typeof(object),
        declaringType: typeof(Form));

    public static readonly BindableProperty IsBusyProperty =
    BindableProperty.Create(
        propertyName: nameof(IsBusy),
        returnType: typeof(bool),
        defaultValue: false,
        declaringType: typeof(Form));

    public static readonly BindableProperty DisplayPlaceholdersProperty =
    BindableProperty.Create(
        propertyName: nameof(DisplayPlaceholders),
        returnType: typeof(bool),
        defaultValue: true,
        declaringType: typeof(Form));

    public static readonly BindableProperty FieldsProperty =
    BindableProperty.Create(
        propertyName: nameof(Fields),
        returnType: typeof(List<FormField>),
        defaultValue: new List<FormField>(),
        declaringType: typeof(Form));

    public static readonly BindableProperty InstanceProperty =
    BindableProperty.Create(
        propertyName: nameof(Instance),
        defaultValue: null,
        returnType: typeof(object),
        declaringType: typeof(Form));

    public static readonly BindableProperty DisplayValidationSummaryProperty =
    BindableProperty.Create(
        propertyName: nameof(DisplayValidationSummary),
        returnType: typeof(bool),
        defaultValue: true,
        declaringType: typeof(Form));

    public static readonly BindableProperty TitleProperty =
    BindableProperty.Create(
        propertyName: nameof(Title),
        returnType: typeof(string),
        declaringType: typeof(Form));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    private BorderItem ValidationSummary = new BorderItem()
    {
        ShapeCornerRadius = 10,
        Padding = 5,
        BackgroundColor = Color.Parse("#0dcaf0")
    };

    public bool DisplayValidationSummary
    {
        get => (bool)GetValue(DisplayValidationSummaryProperty);
        set => SetValue(DisplayValidationSummaryProperty, value);
    }
    public object Instance
    {
        get => (object)GetValue(InstanceProperty);
        set => SetValue(InstanceProperty, value);
    }
    public List<FormField> Fields
    {
        get => (List<FormField>)GetValue(FieldsProperty);
        set => SetValue(FieldsProperty, value);
    }
    public bool DisplayPlaceholders
    {
        get => (bool)GetValue(DisplayPlaceholdersProperty);
        set => SetValue(DisplayPlaceholdersProperty, value);
    }
    public bool IsBusy
    {
        get => (bool)GetValue(IsBusyProperty);
        set
        {
            SetValue(IsBusyProperty, value);
            Ready = !value;
        }
    }

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public bool DisplayLabels
    {
        get => (bool)GetValue(DisplayLabelsProperty);
        set => SetValue(DisplayLabelsProperty, value);
    }
    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }
    public Type SourceType
    {
        get => (Type)GetValue(SourceTypeProperty);
        set => SetValue(SourceTypeProperty, value);
    }

    Button _button = new()
    {
        VerticalOptions = LayoutOptions.EndAndExpand,
        Text = "Submit"
    };
    public Button SubmitButton
    {
        get { return _button; }
        set { SetProperty(ref _button, value); }
    }

    protected StackLayout baseLayout;
    protected StackLayout mainLayout;
    protected Grid ButtonsContainer;

    bool _isValid;
    public bool IsValid
    {
        get { return _isValid; }
        private set { SetProperty(ref _isValid, value); }
    }

    bool _ready = true;
    public bool Ready
    {
        get { return _ready; }
        private set { SetProperty(ref _ready, value); }
    }

    ModelState _modelState;
    public ModelState ModelState
    {
        get { return _modelState; }
        private set { SetProperty(ref _modelState, value); }
    }

    ObservableCollection<ValidationResult> _validationResults = new();
    public ObservableCollection<ValidationResult> ValidationResults
    {
        get { return _validationResults; }
        private set { SetProperty(ref _validationResults, value); }
    }

    ObservableCollection<Label> _labels = new();
    public ObservableCollection<Label> Labels
    {
        get { return _labels; }
        private set { SetProperty(ref _labels, value); }
    }

    ObservableCollection<FormField> _formFields = new();
    public ObservableCollection<FormField> FormFields
    {
        get { return _formFields; }
        private set { SetProperty(ref _formFields, value); }
    }

    private CustomAttributeData GetPropertyCustomAttribute(PropertyInfo property, string attributeName)
    {
        return (property.CustomAttributes != null && property.CustomAttributes.Any()) ?
            property.CustomAttributes.FirstOrDefault(a => a.AttributeType.Name == attributeName) : default;
    }

    private FormField FindFormField(PropertyInfo property)
    {
        return Fields!.FirstOrDefault(f => f.Name == property.Name);
    }
    private string GetPropertyDisplayName(PropertyInfo property)
    {
        string result = property.Name;
        var attrs = property.CustomAttributes;
        //find display attr
        if (attrs != null && attrs.Any())
        {
            var displayAttribute = attrs.FirstOrDefault(a => a.AttributeType.Name == "DisplayAttribute");
            if (displayAttribute != null)
            {
                result = displayAttribute.NamedArguments.FirstOrDefault(a => a.MemberName == "Name").TypedValue.Value.ToString();
            }
        }
        return result;
    }

    private void UpdateBinding(PropertyInfo property, View view)
    {
        if(view is InputView)
        {
            view.SetBinding(InputView.TextProperty, new Binding(property.Name, source: Instance));
        }
        if (view is CheckBox)
        {
            view.SetBinding(CheckBox.IsCheckedProperty, new Binding(property.Name, source: Instance));
        }
        if (view is DatePicker)
        {
            view.SetBinding(DatePicker.DateProperty, new Binding(property.Name, source: Instance));
        }
        if (view is TimePicker)
        {
            view.SetBinding(TimePicker.TimeProperty, new Binding(property.Name, source: Instance));
        }
    }
    private View GetViewByDataType(PropertyInfo property)
    {
        View result = null;

        //Property values and attributes
        var propertyDisplayName = !string.IsNullOrEmpty(GetPropertyDisplayName(property)) ? GetPropertyDisplayName(property) : property.Name;
        var propertyType = property.PropertyType.Name;
        var propertyDataTypeAttribute = GetPropertyCustomAttribute(property, nameof(DataTypeAttribute));
        var propertyDataType = propertyDataTypeAttribute?.ConstructorArguments?.FirstOrDefault();
        
        Entry entry = new();

        entry.SetBinding(InputView.TextProperty, new Binding(property.Name, source: Instance));
        if (propertyDataType != null)
        {
            switch (propertyDataType.Value.Value)
            {
                case (int)DataType.Password:
                    entry.IsPassword = true;
                    break;
            }
        }

        switch (propertyType)
        {
            case nameof(String):
                result = entry;
            break;
            case nameof(Double):
                entry.Keyboard = Keyboard.Numeric;
                result = entry;
            break;
            case nameof(Int16):
                entry.Keyboard = Keyboard.Numeric;
                result = entry;
            break;
            case nameof(Int32):
                entry.Keyboard = Keyboard.Numeric;
                result = entry;
            break;
            case nameof(Int64):
                entry.Keyboard = Keyboard.Numeric;
                result = entry;
                break;
            case nameof(Int128):
                entry.Keyboard = Keyboard.Numeric;
                result = entry;
            break;
            case nameof(Boolean):
                var layout = new StackLayout() { Orientation = StackOrientation.Horizontal, HeightRequest = 50};
                var checkBox = new CheckBox();

                checkBox.SetBinding(CheckBox.IsCheckedProperty, new Binding(property.Name, source: Instance));

                layout.Add(checkBox);
                layout.Add(new Label() { Text = propertyDisplayName, VerticalOptions = LayoutOptions.Center});
                result = layout;
            break;
            case nameof(DateTime):
                var input = new DatePicker();
                input.SetBinding(DatePicker.DateProperty, new Binding(property.Name, source: Instance));
                input.Date = DateTime.Today;
                result = input;
                break;
            case nameof(TimeSpan):
                var timePicker = new TimePicker();
                timePicker.SetBinding(TimePicker.TimeProperty, new Binding(property.Name, source: Instance));
                timePicker.Time = DateTime.Now.TimeOfDay;
                result = timePicker;
                break;
        }
        //Apply other transformations
        // Placeholder
        if(result is InputView)
        {
            (result as InputView).Placeholder = propertyDisplayName;
        }
        return result;
    }

    static void ButtonTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable != null && bindable is Form)
        {
            (bindable as Form).SubmitButton.Text = newValue.ToString();
        }
    }

    public delegate void ClickedEventHandler(object sender, SubmitEventArgs e);

    // Declare the event.
    public event ClickedEventHandler Submit;

    /// <summary>
    /// Default constructor
    /// </summary>
    public Form()
	{
        InitializeComponent();

        ButtonsContainer = new()
        {
            HorizontalOptions = LayoutOptions.EndAndExpand,
            ColumnDefinitions = new()
            {
                new ColumnDefinition(GridLength.Auto),
                new ColumnDefinition(GridLength.Auto)
            }
        };

        SubmitButton.Clicked += SubmitButton_Clicked;
        SubmitButton.Clicked += (sender, args) =>

        //Assgin second click event to submit button
        Submit?.Invoke(SubmitButton, new SubmitEventArgs(this, new ModelState() 
        { 
            ValidationResults = ValidationResults, 
            Model = Instance
        }));
        SubmitButton.SetBinding(Button.IsEnabledProperty, new Binding(nameof(Ready), source: this));
        SubmitButton.SetBinding(Button.CommandProperty, new Binding(nameof(Command), source: this));
        SubmitButton.SetBinding(Button.CommandParameterProperty, new Binding(nameof(CommandParameter), source: this));
    }

    private void ConfigureFormFields()
    {
        try
        {
            //Create instance of source type
            var sourceType = SourceType;
            
            //Layputs
            
            if (sourceType != null)
            {
                var properties = sourceType.GetProperties();
                Instance ??= Activator.CreateInstance(sourceType);

                SubmitButton.CommandParameter = Instance;

                if (properties != null && properties.Any())
                    foreach (var property in properties)
                    {
                        var field = new FormField
                        {
                            Name = property.Name,
                            Label = new Label() { Text = GetPropertyDisplayName(property), IsVisible = DisplayLabels },
                            ErrorLabel = new ErrorLabel() { FontSize = 10, Name = $"{sourceType.Name}_{property.Name}Error" },
                            ErrorImage = new ()
                            {
                                Source = "ic_error_outline_white_48dp.png",
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.End,
                                HeightRequest = 20,
                                WidthRequest = 20,
                                Name = $"Image{sourceType.Name}_{property.Name}Error"
                            },
                            Input = GetViewByDataType(property)
                        };

                        if (field.Input is InputView && !DisplayPlaceholders)
                            (field.Input as InputView).Placeholder = string.Empty;

                        //Override form fields with used defined custom fields
                        if (Fields.Any(f => f.Name == field.Name && (f.Type != null && f.Type == sourceType)))
                        {
                            var userField = Fields.FirstOrDefault(f => f.Name == field.Name);
                            if (userField.Label != null)
                            {
                                field.Label = userField.Label;
                            }
                            if (userField.ErrorLabel != null)
                            {
                                field.ErrorLabel = userField.ErrorLabel;
                            }
                            if (userField.Input != null)
                            {
                                field.Input = userField.Input;

                                //Update custom field binding
                                UpdateBinding(property, field.Input);
                            }
                            field.FieldStatus = userField.FieldStatus;
                        }

                        FormFields.Add(field);

                        //Add elements to container layout
                        if (field.FieldStatus == FieldStatus.Default || field.FieldStatus == FieldStatus.IgnoreValidation)
                        {
                            //Configure entry validation area
                            Grid entryContainer = new ();

                            entryContainer.Add(field.Input);
                            entryContainer.Add(field.ErrorImage);

                            BorderItem entryBorder = new() 
                            { 
                                ShapeCornerRadius = 5,
                                Stroke = Color.Parse("Transparent")
                            };
                            entryBorder.Content = entryContainer;
                            mainLayout.Add(field.Label);
                            //mainLayout.Add(field.Input);
                            mainLayout.Add(entryBorder);
                            mainLayout.Add(field.ErrorLabel);
                        }
                    }
                baseLayout.Add(mainLayout);
                
                ButtonsContainer.Add(SubmitButton);
                
                baseLayout.Add(ButtonsContainer);
                //baseLayout.Add(PrevButton);
                Content ??= baseLayout;
            }
        }
        catch(Exception e)
        {
            Debug.WriteLine(e.StackTrace);
        }
    }
    private void ContentView_Loaded(object sender, EventArgs e)
    {
        baseLayout = new() { Spacing = 5 };
        
        //Set title
        if (!string.IsNullOrEmpty(Title))
        {
            Label title = new();
            title.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));
            BoxView boxView = new BoxView() { HeightRequest = 1, BackgroundColor = Color.Parse("#6E6E6E") };
            VerticalStackLayout titleLayout = new VerticalStackLayout();
            titleLayout.Add(title);
            titleLayout.Add(boxView);
            baseLayout.Add(titleLayout);
        }
        
        mainLayout = new() { Spacing = 5 };
        ConfigureFormFields();
    }

    /// <summary>
    /// First click event triggered by the submit button
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            IsValid = FormValidation.IsFormValid(Instance, mainLayout);
            ValidationResults = new(FormValidation.Validate(Instance, mainLayout));
            ModelState = new ModelState()
            {
                Model = Instance,
                ValidationResults = ValidationResults
            };
            
            if (!IsValid && DisplayValidationSummary)
            {
                Label errorLabel = new Label() 
                { 
                    TextColor = Color.Parse("#dc3545"), 
                    FontAttributes = FontAttributes.Italic, 
                    FontSize = 10,
                    VerticalOptions = LayoutOptions.Center
                };
                StringBuilder sb = new StringBuilder();
                foreach (var result in ValidationResults)
                {
                    sb.AppendLine($"* {result.ErrorMessage}");
                }
                errorLabel.Text = sb.ToString();
                ValidationSummary.Content = errorLabel;

                if (!mainLayout.Children.Any(l => l.GetType() == typeof(BorderItem)))
                    mainLayout.Add(ValidationSummary);
                
            }
            else
            {
                ValidationSummary.IsVisible = false;
            }
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex.StackTrace);
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
}

public class ErrorImage: Image
{
    public string Name { get; set; }
    public ErrorImage()
    {
        IsVisible = false;
    }
}

public class ErrorLabel : Label
{
    public string Name {  get; set; }
    public ErrorLabel()
    {
        TextColor = Color.Parse("#ff4c4b");
        IsVisible = false;
    }
}

public sealed class ModelState
{
    public object Model { get; set; }
    public IEnumerable<ValidationResult> ValidationResults;
}
public sealed class SubmitEventArgs
{
    public SubmitEventArgs(Form instance, ModelState modelState) 
    { 
        Instance = instance;
        ModelState = modelState;
    }
    public Form Instance { get; } // readonly
    public ModelState ModelState { get; } // readonly
}
internal class CaptionLabel : Label
{
    public CaptionLabel() { }
}