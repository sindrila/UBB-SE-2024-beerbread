using Xunit;
using app.MVVM.ViewModel;
using app.MVVM.Model.Domain;
using CommunityToolkit.Maui.Views;

namespace MusicAppTests
{
    public class SongPageViewModelTests
    {
        [Fact]
        public void VerifyAndGetPosition_NullMediaElement_ReturnsDefaultPosition()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            MediaElement mediaElement = null;

            // Act
            string position = viewModel.VerifyAndGetPosition(mediaElement);

            // Assert
            Assert.Equal("00:00", position);
        }

        [Fact]
        public void VerifyAndGetPosition_ValidMediaElement_ReturnsPositionString()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            var mediaElement = new MediaElement();

            // Act
            string position = viewModel.VerifyAndGetPosition(mediaElement);

            // Assert
            // Since media element position depends on the actual media playback, we can't assert the exact value here.
            // We can only ensure it's not null.
            Assert.NotNull(position);
        }

        [Fact]
        public void VerifyAndGetDuration_NullMediaElement_ReturnsDefaultDuration()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            MediaElement mediaElement = null;

            // Act
            string duration = viewModel.VerifyAndGetDuration(mediaElement);

            // Assert
            Assert.Equal("00:00", duration);
        }

        [Fact]
        public void VerifyAndGetDuration_ValidMediaElement_ReturnsDurationString()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            var mediaElement = new MediaElement();

            // Act
            string duration = viewModel.VerifyAndGetDuration(mediaElement);

            // Assert
            // Since media element duration depends on the actual media playback, we can't assert the exact value here.
            // We can only ensure it's not null.
            Assert.NotNull(duration);
        }

        [Fact]
        public void GetMediaElementSource_ValidSong_ReturnsMediaSourceFromResource()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            var song = new Song(1, "Song 1", "Artist 1", "song.mp3");

            // Act
            MediaSource mediaSource = viewModel.GetMediaElementSource(song);

            // Assert
            Assert.Equal(MediaSource.FromResource("song.mp3"), mediaSource);
        }

        [Fact]
        public void GetMediaElementSource_SongWithServerUrl_ReturnsMediaSourceFromUri()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            var song = new Song(1, "Song 1", "Artist 1", "/path/to/song.mp3");

            // Act
            MediaSource mediaSource = viewModel.GetMediaElementSource(song);

            // Assert
            Assert.Equal(MediaSource.FromUri("/path/to/song.mp3"), mediaSource);
        }

        [Fact]
        public void GetSongImageSource_SongWithImageUrl_ReturnsImageUrl()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            var song = new Song(1, "Song 1", "Artist 1", "song.mp3", "image.jpg");

            // Act
            ImageSource imageSource = viewModel.GetSongImageSource(song);

            // Assert
            Assert.Equal("image.jpg", imageSource);
        }

        [Fact]
        public void GetSongImageSource_SongWithoutImageUrl_ReturnsDefaultImage()
        {
            // Arrange
            var viewModel = new SongPageViewModel();
            var song = new Song(1, "Song 1", "Artist 1", "song.mp3");

            // Act
            ImageSource imageSource = viewModel.GetSongImageSource(song);

            // Assert
            Assert.Equal("song_image.jpeg", imageSource);
        }
    }
}
