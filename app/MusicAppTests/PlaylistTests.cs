using Xunit;
using app.MVVM.Model.Domain;
using System.Collections.Generic;

namespace MusicAppTests
{
    public class PlaylistTests
    {
        [Fact]
        public void EmptyPlaylist_EmptySongs_ReturnsTrue()
        {
            // Arrange
            int id = 1;
            int owner = 1;
            string name = "Test Playlist";
            var playlist = new Playlist(id, owner, name);

            // Act
            bool result = playlist.EmptyPlaylist();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void EmptyPlaylist_NonEmptySongs_ReturnsFalse()
        {
            // Arrange
            int id = 1;
            int owner = 1;
            string name = "Test Playlist";
            var playlist = new Playlist(id, owner, name);
            playlist.AddSong(123); // Adding a dummy song

            // Act
            bool result = playlist.EmptyPlaylist();

            // Assert
            Assert.False(result);
        }
    }
}
