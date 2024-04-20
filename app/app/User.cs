﻿namespace app
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualBasic.FileIO;

    public class User : Account, Interfaces.IUser
    {
        public Playlist history { get; set; }

        public Playlist likedSongs { get; set; }

        public Playlist blockedSongs { get; set; }

        public List<Playlist> playlists { get; set; }

        public User(Account account) : base(account)
        {
        }
    }
}
