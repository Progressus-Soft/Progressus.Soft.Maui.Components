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
    }

}
