namespace app.src.Main_page
{
    using app.MVVM.Model.Data.Repositories;
    using app.MVVM.Model.Data.ServerHandlers;
    using app.MVVM.Model.Data.Utilities;
    using app.MVVM.Model.Domain;
    using app.MVVM.ViewModel;

    public partial class MainPage : ContentPage
    {
        private readonly SongService songService = new ();
        private readonly User user;
        private readonly MockAnalyticsAPI mockAnalyticsAPI;
        private List<Song> recomendedSongs = [];

        public MainPage(User user)
        {
            this.InitializeComponent();
            this.user = user;
            this.mockAnalyticsAPI = new (user);
            this.LoadSongs();
        }

        private void LoadSongs()
        {
            this.recomendedSongs = this.songService.GetSongsWithIds(this.mockAnalyticsAPI.GetRecomendedSongs(5));
            Task.Delay(3);
            this.DisplaySongs(this.recomendedSongs);
        }

        private void OnSongTap(object sender, EventArgs e)
        {
            int id = int.Parse(((HorizontalStackLayout)sender).ClassId);
            src.Song_page.SongPage songPage = new (this.recomendedSongs[id]);
            this.Navigation.PushAsync(songPage);
        }

        private void DisplaySongs(List<Song> songs)
        {
            int row = 0;
            int col = 0;
            int crt = 0;
            foreach (Song song in songs)
            {
                // create song frame
                Frame songFrame = new ()
                {
                    ClassId = song.Id.ToString(),
                    Margin = new Thickness(5),
                    Padding = new Thickness(0),
                    CornerRadius = 15,
                    BackgroundColor = Color.FromArgb("#33436369"),
                    BorderColor = Color.FromArgb("#00000000"),
                };

                // create stack layout
                HorizontalStackLayout stackLayout = new ()
                {
                    ClassId = crt.ToString(),
                };
                crt++;

                // create image frame
                Frame imageFrame = new ()
                {
                    CornerRadius = 10,
                    HeightRequest = 60,
                    WidthRequest = 60,
                    BackgroundColor = Color.FromArgb("#00000000"),
                    BorderColor = Color.FromArgb("#00000000"),
                };

                // create image
                string img_path = song.UrlImage;
                if (img_path == string.Empty)
                {
                    img_path = "song_image.jpeg";
                }
                else
                {
                    img_path = SongFilesServerPathGenerator.GetPngPath() + img_path;
                }

                Image image = new ()
                {
                    Source = img_path,
                    Margin = new Thickness(-20),
                    Aspect = Aspect.AspectFill,
                };

                // add image to image frame
                imageFrame.Content = image;
                stackLayout.Add(imageFrame);

                // add song name to stack layout
                Label songName = new()
                {
                    Text = song.Name,
                    FontSize = 12,
                    FontFamily = "NunitoSans",
                    FontAttributes = FontAttributes.Bold,
                    TextColor = Color.FromArgb("#ffffffff"),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    LineBreakMode = LineBreakMode.WordWrap,
                    MaximumWidthRequest = 100,
                };
                stackLayout.Add(songName);

                // add tap gesture recognizer to stack layout
                TapGestureRecognizer tap_fn = new ();
                tap_fn.Tapped += (s, e) => this.OnSongTap(s, e);
                stackLayout.GestureRecognizers.Add(tap_fn);

                songFrame.Content = stackLayout;

                // add song frame to grid
                this.songsGrid.SetRow(songFrame, row);
                this.songsGrid.SetColumn(songFrame, col);
                this.songsGrid.Children.Add(songFrame);

                col++;
                if (col == 2)
                {
                    if (row == 3)
                    {
                        break;
                    }
                    col = 0;
                    row++;
                }
            }
        }

        private void onUserAccountTap(object sender, TappedEventArgs e)
        {
            // naivgate to user account page
            this.DisplayAlert("UserAccountDetails", "Not yet implemented", "OK");
        }

        private void onMusicCreatorTap(object sender, TappedEventArgs e)
        {
            this.DisplayAlert("MusicCreator", "Not yet linked with other semigroup", "OK");
        }

        private void onHomeTap(object sender, TappedEventArgs e)
        {
            this.bottomNavbar_LibraryImage.Source = "library.png";
            this.bottomNavbar_LibraryLabel.TextColor = Color.FromArgb("#ffffff");

            this.bottomNavbar_ExploreImage.Source = "explore.png";
            this.bottomNavbar_ExploreLabel.TextColor = Color.FromArgb("#ffffff");

            this.bottomNavbar_HomeImage.Source = "home_blue.png";
            this.bottomNavbar_HomeLabel.TextColor = Color.FromArgb("#00c2cb");
        }

        private void onExploreTap(object sender, TappedEventArgs e)
        {
            this.bottomNavbar_LibraryImage.Source = "library.png";
            this.bottomNavbar_LibraryLabel.TextColor = Color.FromArgb("#ffffff");

            this.bottomNavbar_ExploreImage.Source = "explore_blue.png";
            this.bottomNavbar_ExploreLabel.TextColor = Color.FromArgb("#00c2cb");

            this.bottomNavbar_HomeImage.Source = "home.png";
            this.bottomNavbar_HomeLabel.TextColor = Color.FromArgb("#ffffff");
        }

        private void onLibraryTap(object sender, TappedEventArgs e)
        {
            PlaylistsPage playlistsPage = new (this.user);
            this.Navigation.PushAsync(playlistsPage);
        }

        private void onSearchTap(object sender, TappedEventArgs e)
        {

        }

        private void onSearchFocus(object sender, FocusEventArgs e)
        {
            // change the backgroung color of the search bar
            this.search_frame.BackgroundColor = Color.FromArgb("#aff1ff");

            // replace the search icon ion the botton nav with the blue version
            this.bottomNavbar_ExploreImage.Source = "explore_blue.png";
            this.bottomNavbar_ExploreLabel.TextColor = Color.FromArgb("#00c2cb");

            // replace the home with its white version
            this.bottomNavbar_HomeImage.Source = "home.png";
            this.bottomNavbar_HomeLabel.TextColor = Color.FromArgb("#ffffff");
        }

        private void onSearchUnFocus(object sender, FocusEventArgs e)
        {
            // change the backgroung color of the search bar
            this.search_frame.BackgroundColor = Color.FromArgb("#d9d9d9");

            // replace the search icon ion the botton nav with the blue version
            this.bottomNavbar_ExploreImage.Source = "explore.png";
            this.bottomNavbar_ExploreLabel.TextColor = Color.FromArgb("#ffffff");

            // replace the home with its white version
            this.bottomNavbar_HomeImage.Source = "home_blue.png";
            this.bottomNavbar_HomeLabel.TextColor = Color.FromArgb("#00c2cb");
        }

    }
}