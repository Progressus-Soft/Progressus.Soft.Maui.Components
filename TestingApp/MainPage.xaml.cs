using CommunityToolkit.Mvvm.Input;
using Progressus.Soft.Maui.Components;
using Progressus.Soft.Maui.Components.Extensions;
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

        [RelayCommand]
        public void AttachAlertToTheParentLayout(object sender)
        {
            if (sender is Element && (sender as Element)?.Parent is not null && (sender as Element)?.Parent is Layout)
                ((sender as Element)?.Parent as Layout)?.AddAlert(new Alert()
                {
                     Title = "From extension method",
                     Message = "Loaded from command parameter",
                     AlertType = AlertType.Information
                });
		}

		private async void Button_Clicked_1(object sender, EventArgs e)
		{
            //AppShell.SetNavBarIsVisible(container, false);
            await Alert.DisplayAlertAsync(Shell.Current.CurrentPage?.Navigation, "From method", "Success alert from DisplayAlertAsync", AlertType.Information);
		}
	}

}
