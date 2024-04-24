using app.MVVM.Model.Data.ServerHandlers;
using app.MVVM.Model.Domain;
using app.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppTests
{
    public class SongActionsViewModelTests
    {
        private readonly ISongActionsViewModel _viewModel;

        public SongActionsViewModelTests()
        {
            _viewModel = new SongActionsViewModel();
        }

        [Fact]
        public void GetSongImageSource_ValidImage_ReturnsCorrectPath()
        {
            // Arrange
            var song = new Song
            {
                UrlImage = "image1.png"
            };

            // Act
            var result = _viewModel.getSongImageSource(song);

            // Assert
            var expected = "http://localhost:1444/api/source/png/image1.png";
            var expectedd= SongFilesServerPathGenerator.GetPngPath() + song.UrlImage;
            Assert.Equal(expected, result.ToString());
            Console.WriteLine(result);
        }

        [Fact]
        public void GetSongImageSource_EmptyImage_ReturnsDefaultPath()
        {
            // Arrange
            var song = new Song
            {
                UrlImage = ""
            };

            // Act
            var result = _viewModel.getSongImageSource(song);

            // Assert
            var expected = "song_image.jpeg";
            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public void GetSongImageSource_NullImage_ReturnsDefaultPath()
        {
            // Arrange
            var song = new Song
            {
                UrlImage = null
            };

            // Act
            var result = _viewModel.getSongImageSource(song);

            // Assert
            var expected = "song_image.jpeg";
            Assert.Equal(expected, result.ToString());
        }

        [Fact]
        public void GetSongImageSource_BlankSpaceImage_ReturnsDefaultPath()
        {
            // Arrange
            var song = new Song
            {
                UrlImage = "   " // Whitespace
            };

            // Act
            var result = _viewModel.getSongImageSource(song);

            // Assert
            var expected = "song_image.jpeg";
            Assert.Equal(expected, result.ToString());
        }
    }
}
