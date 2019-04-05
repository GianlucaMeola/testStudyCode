using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Xunit;

namespace GameEngine.Test
{
    public class BossEnemyTest
    {
        [Fact]
        private void BossHaveTheRightPower()
        {
            //arrange
            Enemy sut =new BossEnemy();

            //act

            //assert
            Assert.Equal(166.67, sut.TotalSpecialAttackPower,2);
        }
    }
}
