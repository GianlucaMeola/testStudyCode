using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorTest
    {
        [Fact]
        public void AcceptHightIncomeApplications()
        {
            //arrange
            var sut = new CreditCardApplicationEvaluator(null);

            var application = new CreditCardApplication {GrossAnnualIncome = 100_000};

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.AutoAccepted, decision);
        }

        [Fact]
        public void ReferYoungApplications()
        {
            //arrange
            var sut = new CreditCardApplicationEvaluator(null);
            var application = new CreditCardApplication {Age = 19};

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }
    }
}
