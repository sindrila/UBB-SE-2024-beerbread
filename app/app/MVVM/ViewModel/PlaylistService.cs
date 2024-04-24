namespace app.MVVM.ViewModel
{
    using app.MVVM.Model.Data.Repositories;
    using app.MVVM.Model.Domain;

    public class PlaylistService
    {
        private ISqlPlaylistRepository playlistRepository;

        public PlaylistService()
        {
            this.playlistRepository = new SqlPlaylistRepository();
        }

        // New constructor to allow injection of ISqlPlaylistRepository
        public PlaylistService(ISqlPlaylistRepository repository)
        {
            this.playlistRepository = repository;
        }

        public List<IPlaylist> GetUserPlaylists(Guid userId)
        {
            return this.playlistRepository.GetUserPlaylists(userId);
        }

        public void AddPlaylist(IPlaylist playlist, Guid id)
        {
            this.playlistRepository.AddPlaylist(playlist, id);
        }
    }
}
