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
			form.Number = Steps.IndexOf(form) + 1;

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