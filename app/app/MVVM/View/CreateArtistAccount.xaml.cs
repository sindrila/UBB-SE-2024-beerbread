namespace app
{
    using app.MVVM.ViewModel;
    using app.src;

    public partial class CreateArtistAccount : ContentPage
    {
        private readonly ICreateArtistAccountViewModel artistAccountViewModel;

        public CreateArtistAccount()
        {
            this.InitializeComponent();
            this.artistAccountViewModel = new CreateArtistAccountViewModel();
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
        }

        private void CreateButton_Clicked(object sender, EventArgs e)
        {
            // Perform create action
            string name = this.ArtistEntryName.Text;
            string username = this.ArtistEntryUsername.Text;
            string country = this.ArtistEntryCountry.Text;
            string dateBirth = this.ArtistEntryDateBirth.Text;
            string email = this.ArtistEntryEmail.Text;
            string password = this.ArtistEntryPassword.Text;

            if (!this.artistAccountViewModel.IsNameValid(name))
            {
                this.DisplayAlert("Name Error", "Can only contain letters", "OK");
                return;
            }

            if (!this.artistAccountViewModel.IsUsernameValid(username))
            {
                this.DisplayAlert("Username Error", "Must be at least 6 characters", "OK");
                return;
            }

            if (!this.artistAccountViewModel.IsCountryValid(country))
            {
                this.DisplayAlert("Country Error", "Can only contain letters", "OK");
                return;
            }

            if (!this.artistAccountViewModel.IsBirthDateValid(dateBirth))
            {
                this.DisplayAlert("Error", "Invalid date of birth format. Please use day/month/year", "OK");
                return;
            }

            if (!this.artistAccountViewModel.IsEmailValid(email))
            {
                this.DisplayAlert("Error", "Invalid email format. Email should end with @yahoo.com or @gmail.com", "OK");
                return;
            }

            if (this.artistAccountViewModel.IsPasswordValid(password))
            {
                this.DisplayAlert("Password Error", "Must be at least 8 characters", "OK");
                return;
            }

            // pop up on screen saying "Account created successfully"
            this.DisplayAlert("Alert", "Account created successfully", "OK");

            // empty the fields
            this.ArtistEntryName.Text = string.Empty;
            this.ArtistEntryUsername.Text = string.Empty;
            this.ArtistEntryCountry.Text = string.Empty;
            this.ArtistEntryDateBirth.Text = string.Empty;
            this.ArtistEntryEmail.Text = string.Empty;
            this.ArtistEntryPassword.Text = string.Empty;
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            LogIn logIn = new ();
            this.Navigation.PushAsync(logIn);
        }
    }
}