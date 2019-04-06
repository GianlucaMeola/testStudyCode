using Xunit;
using System;

namespace GameEngine.Test
{
    public class EnemyFactoryTest
    {
        private EnemyFactory sut;

        public EnemyFactoryTest()
        {
                sut = new EnemyFactory();
        }

        [Fact]
        private void NormalEnemyCreatedByDefault()
        {
            //arrange
            Enemy enemy = sut.Create("Zombie");

            //act

            //assert
            Assert.IsType<NormalEnemy>(enemy);
        }

        [Fact]
        private void IsNotBossByDefault()
        {
            //arrange
            Enemy enemy = sut.Create("Zombie");

            //act

            //assert
            Assert.IsNotType<BossEnemy>(enemy);
        }

        [Fact]
        private void CreateBossNadCheckThatIsBoss()
        {
            //arrange
            Enemy enemy =sut.Create("Zombie King", true);
            //act

            //assert
            Assert.IsType<BossEnemy>(enemy);
        }

        [Fact]
        private void CreateBoss_Cast_CheckTheName()
        {
            //arrange
            Enemy enemy = sut.Create("Zombie King", true);
            //act

            //assert and cast
            Enemy boss = Assert.IsType<BossEnemy>(enemy);

            //assert the cast
            Assert.Equal("Zombie King", boss.Name);
        }

        [Fact]
        private void CreateBoss_CheckAssignableTYpe()
        {
            //arrange
            Enemy enemy = sut.Create("Zombie King", true);

            //act

            //assert
            Assert.IsAssignableFrom<Enemy>(enemy);
        }

        [Fact]
        private void TwoEnemyAreDIfferent()
        {
            //arrange
            Enemy enemy1 = sut.Create("Zoombie");
            Enemy enemy2 = sut.Create("Zoombie");


            //act

            //assert
            Assert.NotSame(enemy1, enemy2);
        }

        [Fact]
        private void NoNullNameAccepted()
        {
            //arrange

            //Act

            //Assert
            //Assert.Throws<ArgumentNullException>(() => sut.Create(null));
            Assert.Throws<ArgumentNullException>("name", () => sut.Create(null));
        }

        [Fact]
        private void ValidBossName()
        {
            //arrange

            //act

            //assert
            EnemyCreationException ex =
            Assert.Throws<EnemyCreationException>(() => sut.Create("Zombie", true));

            Assert.Equal("Zombie", ex.RequestedEnemyName);
        }
    }
}
