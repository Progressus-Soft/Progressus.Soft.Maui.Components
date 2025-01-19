using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Progressus.Soft.Maui.Components;

public class FormStep : Form
{
	public static readonly BindableProperty SuccessSubmittedProperty =
	BindableProperty.Create(
		propertyName: nameof(SuccessSubmitted),
		defaultValue: false,
		returnType: typeof(bool),
		declaringType: typeof(FormStep),
		propertyChanged: SuccessSubmitChanged);

	public static readonly BindableProperty ImageSourceProperty =
	BindableProperty.Create(
		propertyName: nameof(ImageSource),
		returnType: typeof(ImageSource),
		declaringType: typeof(FormStep));

	public static readonly BindableProperty DisplayTitleProperty =
	BindableProperty.Create(
		propertyName: nameof(DisplayTitle),
		defaultValue: true,
		returnType: typeof(bool),
		declaringType: typeof(FormStep));

	public bool DisplayTitle
	{
		get => (bool)GetValue(DisplayTitleProperty);
		set => SetValue(DisplayTitleProperty, value);
	}
	public bool SuccessSubmitted
	{
		get => (bool)GetValue(SuccessSubmittedProperty);
		set => SetValue(SuccessSubmittedProperty, value);
	}
	public ImageSource ImageSource
	{
		get => (ImageSource)GetValue(ImageSourceProperty);
		set => SetValue(ImageSourceProperty, value);
	}
	public bool IsFirst { get; private set; }
	public bool RequireSuccessSubmitted { get; set; }
	public FormStep? NextStep { get; set; }
	public FormStep? PrevStep { get; set; }
	public int Number { get; set; }

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
		if (bindable != null)
		{
			var step = (FormStep)bindable;

			step.IsVisible = !(bool)newValue;
			if (step.NextStep != null)
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
			if (RequireSuccessSubmitted)
			{
				IsVisible = !(NextStep != null) && !SuccessSubmitted;
				if (NextStep != null)
					NextStep.IsVisible = SuccessSubmitted;
			}
			else
			{
				IsVisible = !(NextStep != null);
				if (NextStep != null)
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
	protected override void SetTitle()
	{
		HorizontalStackLayout baseTitleLayout = new HorizontalStackLayout();
		VerticalStackLayout titleLayout = new VerticalStackLayout() { HorizontalOptions = LayoutOptions.Center };
		//Set title
		Label titleLabel = new()
		{
			FontAttributes = FontAttributes.Bold,
			HorizontalOptions = LayoutOptions.Center
		};
		Title = !string.IsNullOrEmpty(Title) ? Title : Number.ToString();
		titleLabel.SetBinding(Label.TextProperty, new Binding(nameof(Title), source: this));

		#region Set Crrent step icon and title
		//Set step icon
		if (ImageSource != null)
		{
			BorderedImage stepIcon = new()
			{
				ImageSource = ImageSource,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Center,
				HeightRequest = 40,
				WidthRequest = 40,
				ShapeCornerRadius = 20
			};
			titleLayout.Add(stepIcon);
		}
		
		if(DisplayTitle)
			titleLayout.Add(titleLabel);
		#endregion

		if(NextStep != null)
		{
			BoxView boxView = new BoxView() { HeightRequest = 1, BackgroundColor = Color.Parse("#6E6E6E") };
			//titleLayout.Add(boxView);
		}
		baseTitleLayout.Add(titleLayout);
		baseLayout.Add(baseTitleLayout);
	}
}