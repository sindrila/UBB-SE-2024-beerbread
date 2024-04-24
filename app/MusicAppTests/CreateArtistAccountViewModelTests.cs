using app.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicAppTests
{
    public class CreateArtistAccountViewModelTests
    {
        private readonly ICreateArtistAccountViewModel _viewModel;

        public CreateArtistAccountViewModelTests()
        {
            _viewModel = new CreateArtistAccountViewModel();
        }

        [Fact]
        public void IsNameValid_ValidName_ReturnsTrue()
        {
            Assert.True(this._viewModel.IsNameValid("John"));
        }

        [Fact]
        public void IsNameValid_EmptyOrNullName_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsNameValid(""));
            Assert.False(this._viewModel.IsNameValid(null));
        }

        [Fact]
        public void IsNameValid_NameWithNonLetters_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsNameValid("John123"));
            Assert.False(this._viewModel.IsNameValid("John!"));
        }

        [Fact]
        public void IsUsernameValid_UsernameWithAtLeastSixCharacters_ReturnsTrue()
        {
            Assert.True(this._viewModel.IsUsernameValid("valid1"));
        }

        [Fact]
        public void IsUsernameValid_UsernameWithLessThanSixCharacters_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsUsernameValid("short"));
        }

        [Fact]
        public void IsCountryValid_ValidCountry_ReturnsTrue()
        {
            Assert.True(this._viewModel.IsCountryValid("France"));
        }

        [Fact]
        public void IsCountryValid_CountryWithNonLetters_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsCountryValid("USA1"));
        }


        [Fact]
        public void IsBirthDateValid_ValidDate_ReturnsTrue()
        {
            Assert.True(this._viewModel.IsBirthDateValid("01/01/2000"));
        }

        [Fact]
        public void IsBirthDateValid_InvalidDate_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsBirthDateValid("32/01/2000"));
            Assert.False(this._viewModel.IsBirthDateValid("01/13/2000"));
            Assert.False(this._viewModel.IsBirthDateValid("01/01/2024")); // Out of allowed range
            Assert.False(this._viewModel.IsBirthDateValid("01/01/99")); // Wrong year format
            Assert.False(this._viewModel.IsBirthDateValid("01/01/")); // Missing year
        }

        [Fact]
        public void IsBirthDateValid_InvalidMonth_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsBirthDateValid("01/13/2000")); // Month out of range
            Assert.False(this._viewModel.IsBirthDateValid("01/00/2000")); // Month out of range
        }

        [Fact]
        public void IsBirthDateValid_InvalidYear_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsBirthDateValid("01/01/1989")); // Year too early
            Assert.False(this._viewModel.IsBirthDateValid("01/01/2024")); // Year too late
        }

        [Fact]
        public void IsBirthDateValid_InvalidDateFormat_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsBirthDateValid("01-01-2000")); // Wrong delimiter
            Assert.False(this._viewModel.IsBirthDateValid("01/01/")); // Missing year
            Assert.False(this._viewModel.IsBirthDateValid("01/01/200")); // Incorrect year format
            Assert.False(this._viewModel.IsBirthDateValid("01/01")); // Missing part
        }

        [Fact]
        public void IsBirthDateValid_NonIntegerDateParts_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsBirthDateValid("01/01/abcd")); // Invalid year
            Assert.False(this._viewModel.IsBirthDateValid("ab/01/2000")); // Invalid day
            Assert.False(this._viewModel.IsBirthDateValid("01/xy/2000")); // Invalid month
        }

        [Fact]
        public void IsEmailValid_ValidEmail_ReturnsTrue()
        {
            Assert.True(this._viewModel.IsEmailValid("test@gmail.com"));
            Assert.True(this._viewModel.IsEmailValid("test@yahoo.com"));
        }

        [Fact]
        public void IsEmailValid_InvalidEmail_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsEmailValid("test@hotmail.com")); // Not allowed domain
            Assert.False(this._viewModel.IsEmailValid("invalid.com")); // Missing '@'
            Assert.False(this._viewModel.IsEmailValid("")); // Empty
            Assert.False(this._viewModel.IsEmailValid(null)); // Null
        }

        [Fact]
        public void IsPasswordValid_ValidPassword_ReturnsTrue()
        {
            Assert.True(this._viewModel.IsPasswordValid("password123"));
        }

        [Fact]
        public void IsPasswordValid_ShortPassword_ReturnsFalse()
        {
            Assert.False(this._viewModel.IsPasswordValid("short"));
        }
    }
}
