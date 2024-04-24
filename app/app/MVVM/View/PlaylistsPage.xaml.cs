namespace app.src
{
    using app.MVVM.Model.Data.Repositories;
    using app.MVVM.Model.Data.ServerHandlers;
    using app.MVVM.Model.Domain;
    using app.MVVM.ViewModel;

    public partial class PlaylistsPage : ContentPage
    {
        private User user;
        private List<IPlaylist> playlists;

        public PlaylistsPage(User user)
        {
            this.user = user;
            InitializeComponent();
            LoadPlaylists();
        }

        private void LoadPlaylists()
        {
            PlaylistService playlistService = new();
            List<IPlaylist> playlists = playlistService.GetUserPlaylists(user.Id);
            this.playlists = playlists;
            int crt = 0;
            playlistLayout.Children.Clear();
            foreach (IPlaylist playlist in playlists)
            {

                Frame frame = new Frame
                {
                    CornerRadius = 15,
                    BackgroundColor = Color.FromArgb("#FF000000"),
                    BorderColor = Color.FromArgb("#FF000000"),
                    Padding = new Thickness(0),
                    Margin = new Thickness(5, 5, 5, 5)
                };
                HorizontalStackLayout stackLayout = new HorizontalStackLayout
                {
                    ClassId = crt.ToString()
                };
                crt++;
                Frame imageFrame = new Frame
                {
                    CornerRadius = 10,
                    HeightRequest = 60,
                    WidthRequest = 60,
                    BackgroundColor = Color.FromArgb("#FFFFFFFF"),
                };
                string imgPath = playlist.ImagePath;
                if (imgPath != "")
                {
                    imgPath = SongFilesServerPathGenerator.GetPngPath() + imgPath;
                }
                Image image = new Image
                {
                    Source = imgPath,
                    Aspect = Aspect.AspectFill,
                    Margin = new Thickness(-20),
                };
                imageFrame.Content = image;
                stackLayout.Children.Add(imageFrame);
                Label label = new Label
                {
                    Text = playlist.Name,
                    FontSize = 15,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.FromArgb("#FFFFFFFF"),
                    FontFamily = "NunitoSans",
                    FontAttributes = FontAttributes.Bold,
                    WidthRequest = 200,
                };
                stackLayout.Children.Add(label);

                Frame buttonFrame = new Frame
                {
                    CornerRadius = 10,
                    HeightRequest = 40,
                    WidthRequest = 40,
                    BackgroundColor = Color.FromArgb("#00000000"),
                    BorderColor = Color.FromArgb("#00000000"),
                    Padding = new Thickness(0),
                    Margin = new Thickness(30, 0, 0, 0),
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center,
                    IsClippedToBounds = true,
                };
                Image details = new Image
                {
                    Source = "delete_menu.png",
                    Aspect = Aspect.AspectFit,
                    Margin = new Thickness(-20, 0, 0, 0),
                    HeightRequest = 40,
                    WidthRequest = 20,
                };
                buttonFrame.Content = details;
                TapGestureRecognizer tap_fn_img = new();
                tap_fn_img.Tapped += (s, e) => OnDetailsTap(s, e);
                details.GestureRecognizers.Add(tap_fn_img);

                TapGestureRecognizer tap_fn = new();
                tap_fn.Tapped += (s, e) => OnPlaylistTap(s, e);
                stackLayout.GestureRecognizers.Add(tap_fn);

                stackLayout.Children.Add(buttonFrame);
                frame.Content = stackLayout;
                playlistLayout.Children.Add(frame);
            }
        }

        private void OnDetailsTap(object sender, EventArgs e)
        {
            DisplayAlert("Error", "Details to be implementes", "OK");
            return;
        }

        private void OnPlaylistTap(object sender, EventArgs e)
        {
            int playlistId = Int32.Parse(((HorizontalStackLayout)sender).ClassId);
            IPlaylist playlist = playlists[playlistId];

            SinglePlaylistPage singlePlaylistPage = new SinglePlaylistPage(playlist);
            Navigation.PushAsync(singlePlaylistPage);
        }

        private async Task Exec()
        {
            string result = await DisplayPromptAsync("PlaylistsName", "");

            if (result == null)
                return;

            string action = await DisplayActionSheet("Playlist visability", "Cancel", null, "Private", "Public");
            if (action == null)
            {
                return;
            }

            bool isPrivate = action == "Private";

            PlaylistService playlistService = new();
            IPlaylist playlist = new Playlist(-1, -1, result, isPrivate);

            playlistService.AddPlaylist(playlist, user.Id);
            this.LoadPlaylists();
        }

        private async void AddButtonTapped(object sender, EventArgs e)
        {

            await Exec();
        }

        private void DetailsDelete(object sender, EventArgs e)
        {
            DisplayAlert("Error", "Details to be implementes", "OK");
            return;
        }

        private void BackButtonTapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }



        private void PlaylistDetailPageButton(object sender, EventArgs e)
        {

            DisplayAlert("Error", "Aici o sa fie pagina de detalii playlist.", "OK");
            return;
        }


        private void LibraryPageButtonClicked(object sender, EventArgs e)
        {
        }


        private void SearchPageButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }


        private void HomePageButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }
    }
}