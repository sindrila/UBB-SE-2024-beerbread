namespace app.MVVM.ViewModel
{
    using app.MVVM.Model.Domain;
    using app.MVVM.Model.Data.ServerHandlers;

    public interface ISongActionsViewModel
    {
        ImageSource getSongImageSource(Song song);
    }

    public class SongActionsViewModel : ISongActionsViewModel
    {
        public SongActionsViewModel() { }

        public ImageSource getSongImageSource(Song song)
        {
            if (song.UrlImage != "")
                return SongFilesServerPathGenerator.GetPngPath() + song.UrlImage;
            return "song_image.jpeg";
        }
    }
}
