using System;
using Xunit;
namespace GameEngine.Tests
{
    public class NonPlayerCharacterTest
    {
        /*[Theory]
        [InlineData(0,100)]
        [InlineData(1,99)]
        [InlineData(50,50)]
        [InlineData(101,1)]*/
        [Theory]
        //[MemberData("TestData", MemberType = typeof(InternalHealthDamageTestData))]
        //or
        //[MemberData(nameof(ExternalHealthDamageTestData.TestData), MemberType = typeof(ExternalHealthDamageTestData))]
        //or
        [HealthDamageData]
        public void TakeDamage(int damage, int expectedHealt)
        {
            //arrange
            NonPlayerCharacter sut = new NonPlayerCharacter();
            
            //act
            sut.TakeDamage(damage);

            //assert
            Assert.Equal(expectedHealt, sut.Health);
        }
    }
}
