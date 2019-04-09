using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Xunit;
using Xunit.Abstractions;

namespace GameEngine.Test
{
    [Trait("Category", "EnemyBoss")]
    public class BossEnemyTest
    {
        private readonly ITestOutputHelper _output;

        public BossEnemyTest(ITestOutputHelper output)
        {
            _output = output;
        }


        [Fact]
        private void BossHaveTheRightPower()
        {
            //arrange
            _output.WriteLine("Creating Enemy Boss");
            Enemy sut = new BossEnemy();

            //act

            //assert
            Assert.Equal(166.67, sut.SpecialAttackPower, 2);
        }
    }
}
