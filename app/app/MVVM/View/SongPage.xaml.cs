namespace app.src.Song_page 
{

    using System.Diagnostics;
    using app.MVVM.Model.Data.ServerHandlers;
    using app.MVVM.Model.Domain;
    using app.MVVM.ViewModel;
    using CommunityToolkit.Maui.Views;

    public partial class SongPage : ContentPage
    {
        private ISongPageViewModel songPageViewModel = new SongPageViewModel();

        private bool isPlaying = false;
        private bool isLiked = false;
        private bool noAutoChange = true;
        private Song song;

        public string Position
        {
            get
            {
                return this.songPageViewModel.VerifyAndGetPosition(mediaElement);
            }
        }

        public string Duration
        {
            get
            {
                return this.songPageViewModel.VerifyAndGetDuration(mediaElement);
            }
        }

        public SongPage(Song song)
        {
            BindingContext = this;
            InitializeComponent();
            this.song = song;
            //song_name.Text = song.Name;
            //artist_name.Text = song.Artist;
            Trace.WriteLine(SongFilesServerPathGenerator.GetMp3Path() + song.UrlSong);

            mediaElement.Volume = 0.75;
            mediaElement.Source = this.songPageViewModel.GetMediaElementSource(song);
            songImage.Source = this.songPageViewModel.GetSongImageSource(song);
            songImage.Source = ImageSource.FromUri(new Uri(SongFilesServerPathGenerator.GetPngPath() + song.UrlImage));
        }

        private void mediaElement_loaded(object sender, EventArgs e)
        {
            mediaElement.PositionChanged += MediaElement_PositionChanged;
            mediaElement.Source = MediaSource.FromUri(SongFilesServerPathGenerator.GetMp3Path() + song.UrlSong);
            total_song_time.Text = mediaElement.Duration.ToString(@"mm\:ss");
        }

        private void MediaElement_PositionChanged(object? sender, CommunityToolkit.Maui.Core.Primitives.MediaPositionChangedEventArgs e)
        {
            noAutoChange = false;
            progress_bar.Value = (mediaElement.Position).TotalSeconds / (mediaElement.Duration).TotalSeconds;
            noAutoChange = true;
            OnPropertyChanged(nameof(Position));
            total_song_time.Text = mediaElement.Duration.ToString(@"mm\:ss");
        }

        private void onBackButtonTapped(object sender, EventArgs e)
        {
            mediaElement.Stop();
            Navigation.PopAsync();
        }

        private void onSongDetailButtonTapped(object sender, EventArgs e)
        {
            src.Song_actions.SongActions songActions = new(this.song);
            Navigation.PushAsync(songActions);
        }

        private void onDragStartedValueChange(object sender, EventArgs e)
        {
            isPlaying = false;
            mediaElement.Pause();
            progress_bar.ThumbColor = Color.FromArgb("#FF7cffcf");
        }
        private void onDragCompletedValueChange(object sender, EventArgs e)
        {
            mediaElement.SeekTo(TimeSpan.FromSeconds(progress_bar.Value * mediaElement.Duration.TotalSeconds));
            isPlaying = true;
            mediaElement.Play();
            progress_bar.ThumbColor = Color.FromArgb("#FF7cadff");
        }

        //private void onLikedButtonClicked(object sender, TappedEventArgs e)
        //{
        //    if (!isLiked)
        //    {
        //        isLiked = true;
        //        like_button.Source = "liked_button.png";
        //    }
        //    else
        //    {
        //        isLiked = false;
        //        like_button.Source = "liked_unpressed_button.png";
        //    }
        //}
        //private void goBackSongButton(object sender, EventArgs e)
        //{
        //    if (mediaElement.Position.TotalSeconds > 5)
        //    {
        //        mediaElement.SeekTo(TimeSpan.FromSeconds(0));
        //    }
        //}

        //private void PlayPauseButton(object sender, TappedEventArgs e)
        //{
        //    if (!isPlaying)
        //    {
        //        isPlaying = true;
        //        play_one_song_button.Source = "pause_song_button";

        //        try
        //        {
        //            mediaElement.Play();
        //        }
        //        catch (Exception ex)
        //        {
        //            Debug.WriteLine(ex.Message);
        //        }
        //    }
        //    else
        //    {
        //        isPlaying = false;
        //        play_one_song_button.Source = "play_song_button";
        //        mediaElement.Pause();
        //    }

        //}

        //private void goForwardSongButton(object sender, EventArgs e)
        //{
        //    DisplayAlert("Error", "Not implemented yet..", "OK");
        //    return;
        //}
    }
}
