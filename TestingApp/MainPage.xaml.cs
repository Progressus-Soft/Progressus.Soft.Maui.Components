using Progressus.Soft.Maui.Components;
using TestingApp.Flyout;

namespace TestingApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            var fly = new FlyoutSidePage();
            //fly.IsPresented = true;
            Shell.Current.FlyoutContent = fly;
            Shell.Current.FlyoutIsPresented = true;
        }

		private async void Button_Clicked_1(object sender, EventArgs e)
		{
            //AppShell.SetNavBarIsVisible(container, false);
            await Alert.DisplayAlertAsync(Application.Current.MainPage?.Navigation, "From method", "Success alert from DisplayAlertAsync", AlertType.Information);
		}
	}

}
