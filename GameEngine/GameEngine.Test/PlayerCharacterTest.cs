using System;
using Xunit;

namespace GameEngine.Test
{
    public class GameEngineShould
    {
        private PlayerCharacter _sut;

        public GameEngineShould()
        {
               _sut = new PlayerCharacter(); 
        }

        [Fact]
        public void BeInesperiencedWhenNew()
        {
            //arrange
            //PlayerCharacter sut = new PlayerCharacter();

            //assert
            Assert.True(_sut.IsNoob);
        }

        [Theory]
        [InlineData("Sara","Smith")]
        private void FullNameMatch(string FirstName, string LastName)
        {
            //arrange
            _sut.FirstName = FirstName;
            _sut.LastName = LastName;

            //act

            //assert
            Assert.Equal(FirstName +" "+ LastName, _sut.FullName);
            Assert.NotEmpty(_sut.FullName);
        }

        [Theory]
        [InlineData("Sara", "Smith")]
        private void SpacingFullNameTest(string FirstName, string LastName)
        {
            //arrange
            _sut.FirstName = FirstName;
            _sut.LastName = LastName;

            //act

            //assert
            Assert.NotEqual(FirstName + LastName, _sut.FullName);
        }

        [Theory]
        [InlineData("Sara", "Smith")]
        private void FistNameShouldMatch(string FirstName, string LastName)
        {
            //arrange
            _sut.FirstName = FirstName;
            _sut.LastName = LastName;

            //act

            //assert
            Assert.StartsWith(FirstName, _sut.FullName);
        }

        [Theory]
        [InlineData("Sara", "Smith")]
        private void LastNameShouldMatch(string FirstName, string LastName)
        {
            //arrange
            _sut.FirstName = FirstName;
            _sut.LastName = LastName;

            //act

            //assert
            Assert.EndsWith(LastName, _sut.FullName);
        }

        [Theory]
        [InlineData("Sara", "Smith")]
        private void CaseInsensitiveTest(string FirstName, string LastName)
        {
            //arrange
            _sut.FirstName = "sara";
            _sut.LastName = "Smith";

            //act

            //assert
            Assert.Equal(FirstName +" "+ LastName, _sut.FullName, ignoreCase:true);
        }

        [Theory]
        [InlineData("Sara", "Smith")]
        private void ShouldCOntainPartOfTheName(string FirstName, string LastName)
        {
            //arrange
            _sut.FirstName = FirstName;
            _sut.LastName = LastName;

            //act

            //assert
            Assert.Contains( "a Smi", _sut.FullName);
        }

        [Theory]
        [InlineData("Sara", "Smith")]
        private void GeneralRulesToMatchForName(string FirstName, string LastName)
        {
            //arrange
            _sut.FirstName = FirstName;
            _sut.LastName = LastName;

            //act

            //assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", _sut.FullName);
        }

        [Fact]
        private void HeltMaxAtBegining()
        {
            //arrange

            //act

            //assert
            Assert.Equal(100, _sut.Health);
            Assert.NotEqual(0, _sut.Health);
        }

        [Fact]
        private void HealtIncreaseAfterSleeping()
        {
            //arrange

            //act
            _sut.Sleep();

            //assert
            Assert.InRange<int>(_sut.Health, 101, 200);
        }

        [Fact]
        private void NoNickNameByDefault()
        {
            //arrange

            //act

            //assert
            Assert.Null(_sut.Nickname);
            //Assert.NotNull(_sut.FullName);
        }

        [Fact]
        private void ThereIsABowByDefault()
        {
            //arrange

            //act
            
            //assert
            Assert.Contains("Long Bow", _sut.Weapons);
        }

        [Fact]
        private void DontContainMagicSword()
        {
            //arrange

            //act

            //assert
            Assert.DoesNotContain("Magic Sword", _sut.Weapons);
        }

        [Fact]
        private void HaveAtLeastOneSword()
        {
            //arrange

            //act
            
            //assert
            Assert.Contains(_sut.Weapons, weapon => weapon.Contains("Sword"));
        }

        [Fact]
        private void ContainTheFullStartUpKit()

        {
            //arrange
            var weaponKit = new[]
            {
                "Long Bow",
                "Short Bow",
                "Short Sword",
            };

            //act

            //assert
            Assert.Equal(weaponKit, _sut.Weapons);
        }

        [Fact]
        private void NoEmptyWeaponString()
        {
            //arrange

            //act

            //arrange
            Assert.All(_sut.Weapons, weapon => Assert.False(string.IsNullOrWhiteSpace(weapon)));
        }

        [Fact]
        private void SleepShouldRaiseSleptEvent()
        {
            //arrange

            //act

            //assert
            Assert.Raises<EventArgs>(
                handle => _sut.PlayerSlept += handle,
                handle => _sut.PlayerSlept -= handle,
                () => _sut.Sleep()
                );
        }

        [Fact]
        private void ChangeHealtOnDamage()
        {
            //arrange

            //act

            //assert
            Assert.PropertyChanged(_sut, "health", () => _sut.TakeDamage(10));
        }
    }
}
