using Xunit;
using app.MVVM.Model.Domain;

namespace MusicAppTests
{
    public class SongTests
    {
        [Fact]
        public void Constructor_InitializesPropertiesCorrectly()
        {
            // Arrange
            int id = 1;
            string name = "Test Song";
            string artist = "Test Artist";
            string urlSong = "song.mp3";
            string urlImage = "image.jpg";

            // Act
            var song = new Song(id, name, artist, urlSong, urlImage);

            // Assert
            Assert.Equal(id, song.Id);
            Assert.Equal(name, song.Name);
            Assert.Equal(artist, song.Artist);
            Assert.Equal(0, song.Likes);
            Assert.Equal(0, song.TimePlayed);
            Assert.Equal(urlSong, song.UrlSong);
            Assert.Equal(urlImage, song.UrlImage);
        }
    }
}
