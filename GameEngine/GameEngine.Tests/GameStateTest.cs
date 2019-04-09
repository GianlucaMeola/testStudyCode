using Xunit;

namespace GameEngine.Test
{
    public class GameStateTest
    {
        [Fact]
        private void DamageAllPlayerWhenEarthquake()
        {
            //arrange
            var sut = new GameState();

            var player1 = new PlayerCharacter();
            var player2 = new PlayerCharacter();

            sut.Players.Add(player1);
            sut.Players.Add(player2);

            var expectedHealtAfterEarthquake = player1.Health - GameState.EarthquakeDamage;


            //act
            sut.Earthquake();

            //assert
            Assert.Equal(expectedHealtAfterEarthquake, player1.Health);
            Assert.Equal(expectedHealtAfterEarthquake, player2.Health);
        }

        [Fact]
        private void Reset()
        {
            //arrange
            var sut = new GameState();

            var player1 = new PlayerCharacter();
            var player2 = new PlayerCharacter();

            sut.Players.Add(player1);
            sut.Players.Add(player2);

            //act
            sut.Reset();

            //assert
            Assert.Empty(sut.Players);
        }
    }
}
