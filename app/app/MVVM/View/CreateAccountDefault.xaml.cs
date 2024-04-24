namespace app
{
    using app.src;

    public partial class CreateAccountDefault : ContentPage
    {
        public CreateAccountDefault()
        {
            this.InitializeComponent();
        }

        private void UserButton_Clicked(object sender, EventArgs e)
        {
            // Switch to User mode
            CreateUserAccount createUserAccount = new ();
            this.Navigation.PushAsync(createUserAccount);
        }

        private void ArtistButton_Clicked(object sender, EventArgs e)
        {
            // Switch to Artist mode
            CreateArtistAccount createArtistAccount = new ();
            this.Navigation.PushAsync(createArtistAccount);
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            LogIn logIn = new LogIn();
            this.Navigation.PushAsync(logIn);
        }
    }
}