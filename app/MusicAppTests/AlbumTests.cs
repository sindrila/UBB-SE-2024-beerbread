using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using app.MVVM.Model.Domain;

namespace MusicAppTests
{
    public class AlbumTests
    {
        [Fact]
        public void Album_ShouldInitializeWithCorrectProperties()
        {
            // Arrange
            int owner = 1;
            int id = 2;
            string name = "Test Album";
            string description = "This is a test description for the album.";

            // Act
            var album = new Album(owner, id, name, description);

            // Assert
            Assert.Equal(owner, album.Owner);
            Assert.Equal(id, album.Id);
            Assert.Equal(name, album.Name);
            Assert.Equal(description, album.Description);
        }
        [Fact]
        public void Album_ShouldBeEmptyStringIfDescriptionIsEmpty()
        {
            // Arrange
            int owner = 1;
            int id = 2;
            string name = "Test Album";

            // Act
            var album = new Album(owner, id, name, "");

            // Assert
            Assert.Equal("", album.Description);
        }
    }
}
