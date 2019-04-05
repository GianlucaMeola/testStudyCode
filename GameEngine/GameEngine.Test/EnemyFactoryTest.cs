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
    }
}
