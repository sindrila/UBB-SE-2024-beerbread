namespace app.MVVM.ViewModel
{
    using app.MVVM.Model.Domain;
    using CommunityToolkit.Maui.Views;
    using app.MVVM.Model.Data.ServerHandlers;

    public interface ISongPageViewModel
    {
        string VerifyAndGetPosition(MediaElement mediaElement);

        string VerifyAndGetDuration(MediaElement mediaElement);

        MediaSource GetMediaElementSource(Song song);

        ImageSource GetSongImageSource(Song song);
    }

    public class SongPageViewModel : ISongPageViewModel
    {
        public string VerifyAndGetPosition(MediaElement mediaElement)
        {
            if (mediaElement != null)
            {
                return mediaElement.Position.ToString(@"mm\:ss");
            }

            return "00:00";
        }

        public string VerifyAndGetDuration(MediaElement mediaElement)
        {
            if (mediaElement != null)
            {
                return mediaElement.Duration.ToString(@"mm\:ss");
            }

            return "00:00";
        }

        public MediaSource GetMediaElementSource(Song song)
        {
            if (!song.UrlSong.StartsWith("/"))
            {
                return MediaSource.FromResource(song.UrlSong);
            }

            return MediaSource.FromUri(SongFilesServerPathGenerator.GetMp3Path() + song.UrlSong);
        }

        public ImageSource GetSongImageSource(Song song)
        {
            if (song.UrlImage != "")
            {
                return song.UrlImage;
            }

            return "song_image.jpeg";
        }
    }
}
