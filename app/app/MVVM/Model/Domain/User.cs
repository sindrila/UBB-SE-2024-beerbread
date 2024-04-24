namespace app.MVVM.Model.Domain
{
    using System.Collections.Generic;

    public class User : Account
    {
        public Playlist History { get; set; }

        public Playlist LikedSongs { get; set; }

        public Playlist BlockedSongs { get; set; }

        public List<Playlist> Playlists { get; set; }

        public User(Account account)
            : base(account)
        {
            Playlists = new List<Playlist>();
        }
    }
}
