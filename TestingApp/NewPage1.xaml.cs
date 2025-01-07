using Progressus.Soft.Maui.Components;

namespace TestingApp;

public partial class NewPage1 : ContentPage
{
	public NewPage1()
	{
		InitializeComponent();
	}

	private async void Button_Clicked_1(object sender, EventArgs e)
	{
		ContentPage container = new()
		{
			BackgroundColor = Color.Parse("Transparent"),
			//Padding = new Thickness(0, 10)

		};
		Alert alert = new()
		{
			Message = "Text",
			AlertType = AlertType.Danger,
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center,
		};
		container.Content = alert;
		await Application.Current.MainPage.Navigation.PushModalAsync(container);
	}
}