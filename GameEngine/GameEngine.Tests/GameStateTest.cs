using GameEngine.Tests;
using  Xunit.Abstractions;
using Xunit;

namespace GameEngine.Test
{
    public class GameStateTest : IClassFixture<GameStateFixture>
    {
        private readonly GameStateFixture _gameStateFixture;
        private readonly ITestOutputHelper _output;

        public GameStateTest(GameStateFixture gameStateFixture, ITestOutputHelper output)
        {
            _gameStateFixture = gameStateFixture;
            _output = output;
        }

        [Fact]
        private void DamageAllPlayerWhenEarthquake()
        {
            //arrange
            _output.WriteLine($"GameState ID={_gameStateFixture.State.Id}");
            var player1 = new PlayerCharacter();
            var player2 = new PlayerCharacter();

            _gameStateFixture.State.Players.Add(player1);
            _gameStateFixture.State.Players.Add(player2);

            var expectedHealtAfterEarthquake = player1.Health - GameState.EarthquakeDamage;


            //act
            _gameStateFixture.State.Earthquake();

            //assert
            Assert.Equal(expectedHealtAfterEarthquake, player1.Health);
            Assert.Equal(expectedHealtAfterEarthquake, player2.Health);
        }

        [Fact]
        private void Reset()
        {
            //arrange
            _output.WriteLine($"GameState ID={_gameStateFixture.State.Id}");

            var player1 = new PlayerCharacter();
            var player2 = new PlayerCharacter();

            _gameStateFixture.State.Players.Add(player1);
            _gameStateFixture.State.Players.Add(player2);

            //act
            _gameStateFixture.State.Reset();

            //assert
            Assert.Empty(_gameStateFixture.State.Players);
        }
    }
}
