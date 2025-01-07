using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Progressus.Soft.Maui.Components;

public partial class MultiStepForm : ContentView, INotifyPropertyChanged
{
    

    public static readonly BindableProperty StepsProperty =
    BindableProperty.Create(
        propertyName: nameof(Steps),
        returnType: typeof(IList<FormStep>),
        defaultValue: new List<FormStep>(),
        declaringType: typeof(Form));
    public IList<FormStep> Steps
    {
        get => (IList<FormStep>)GetValue(StepsProperty);
        set => SetValue(StepsProperty, value);
    }
    public StackLayout Layout { get; private set; }
    private void ContentView_Loaded(object sender, EventArgs e)
    {
        Grid layout = new ();
        foreach(var form in Steps)
        {
            //Detects if current form is the first in the stps list
            var first = Steps.IndexOf(form) == 0;

            form.SetFirst(first);
            form.IsVisible = first;

            form.NextStep = Steps.SkipWhile(x => x.Id != form.Id).Skip(1)!.FirstOrDefault();//.DefaultIfEmpty(Steps[0]).FirstOrDefault();
            form.PrevStep = Steps.TakeWhile(x => x.Id != form.Id).Take(1)!.LastOrDefault();//.DefaultIfEmpty(Steps[Steps.Count - 1]).LastOrDefault();

            //Enable prev button
            form.PrevButton.IsVisible = form.PrevStep != null;
            layout.Add(form);
        }
        Content ??= layout;
    }
    /// <summary>
    /// Default constructor
    /// </summary>
    public MultiStepForm()
	{
        InitializeComponent();
        Layout = new();
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

public class FormStep: Form
{
    public static readonly BindableProperty SuccessSubmittedProperty =
    BindableProperty.Create(
        propertyName: nameof(SuccessSubmitted),
        defaultValue: false,
        returnType: typeof(bool),
        declaringType: typeof(FormStep),
        propertyChanged: SuccessSubmitChanged);
    public bool SuccessSubmitted
    {
        get => (bool)GetValue(SuccessSubmittedProperty);
        set => SetValue(SuccessSubmittedProperty, value);
    }
    public bool IsFirst {  get; private set; }
    public bool RequireSuccessSubmitted {  get; set; }
    public FormStep? NextStep { get; set; }
    public FormStep? PrevStep { get; set; }

    Button _prevButton = new()
    {
        VerticalOptions = LayoutOptions.StartAndExpand,
        BackgroundColor = Color.Parse("Transparent"),
        IsVisible = false,
        Text = "Back"
    };
    public Button PrevButton
    {
        get { return _prevButton; }
        set { SetProperty(ref _prevButton, value); }
    }
    private static void SuccessSubmitChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if(bindable != null)
        {
            var step = (FormStep)bindable;

            step.IsVisible = !(bool)newValue;
            if(step.NextStep != null)
                step.NextStep.IsVisible = (bool)newValue;
        }
    }
    public FormStep SetFirst(bool first)
    {
        IsFirst = first;
        return this;
    }
    public FormStep()
    {
        SubmitButton.Clicked += SubmitButton_Clicked!;
        PrevButton.Clicked += PrevButton_Clicked!;
        ButtonsContainer.Add(PrevButton);
        Grid.SetColumn(SubmitButton, 1);
    }

    private void SubmitButton_Clicked(object sender, EventArgs e)
    {
        if (IsValid)
        {
            if(RequireSuccessSubmitted )
            {
                IsVisible = !(NextStep != null) && !SuccessSubmitted;
                if (NextStep != null)
                    NextStep.IsVisible = SuccessSubmitted;
            }
            else
            {
                IsVisible = !(NextStep != null);
                if(NextStep != null)
                    NextStep.IsVisible = true;
            }
        }
    }

    private void PrevButton_Clicked(object sender, EventArgs e)
    {
        IsVisible = !(PrevStep != null);
        if (PrevStep != null)
            PrevStep.IsVisible = true;
    }
}