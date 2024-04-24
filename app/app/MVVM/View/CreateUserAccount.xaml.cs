namespace app
{
    using System;
    using app.MVVM.ViewModel;

    public partial class CreateUserAccount : ContentPage
    {
        private readonly ICreateUserAccountViewModel createUserAccountViewModel = new CreateUserAccountViewModel();

        public CreateUserAccount()
        {
            this.InitializeComponent();
        }

        private void UserButton_Clicked(object sender, EventArgs e)
        {
            // Switch to User mode
        }

        private void ArtistButton_Clicked(object sender, EventArgs e)
        {
            // Switch to Artist mode
            CreateArtistAccount createArtistAccount = new ();
            this.Navigation.PushAsync(createArtistAccount);
        }

        private void CreateButton_Clicked(object sender, EventArgs e)
        {
            string username = this.UserEntryUsername.Text;
            string password = this.UserEntryPassword.Text;
            string email = this.UserEntryEmail.Text;

            if (!this.createUserAccountViewModel.IsUsernameLengthValid(username))
            {
                this.DisplayAlert("Username Error", "Must be at least 6 characters", "OK");
                return;
            }

            if (!this.createUserAccountViewModel.IsPasswordLengthValid(password))
            {
                this.DisplayAlert("Password Error", "Must be at least 8 characters", "OK");
                return;
            }

            if (!this.createUserAccountViewModel.IsEmailValid(email))
            {
                this.DisplayAlert("Email Error", "Should end with @gmail.com or @yahoo.com", "OK");
                return;
            }

            if (this.createUserAccountViewModel.CreateUserAccount(email, username, password))
            {
                this.DisplayAlert("Success", "Account created successfully", "OK");
                this.Navigation.PushAsync(new src.LogIn());
            }
            else
            {
                this.DisplayAlert("Error", "Account creation failed", "OK");
            }

            this.UserEntryUsername.Text = string.Empty;
            this.UserEntryPassword.Text = string.Empty;
            this.UserEntryEmail.Text = string.Empty;
        }

        private void LoginButton_Clicked(object sender, EventArgs e)
        {
            src.LogIn logIn = new ();
            this.Navigation.PushAsync(logIn);
        }
    }
}
