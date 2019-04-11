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
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

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
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey);
            mockValidator.DefaultValue = DefaultValue.Mock;

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication {Age = 19};

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void DeclineLowIncomeApplications()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            //hardcoded
            //mockValidator.Setup(x => x.IsValid("x")).Returns(true);
            //it class for a more general result and methods
            //method for any string ever true even if empty
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //method for string that start with x
            //mockValidator.Setup(x => x.IsValid(It.Is<string>(number => number.StartsWith('x')))).Returns(true);
            //method for strings that contains x
            //mockValidator.Setup(x => x.IsValid(It.IsIn("x","y","z"))).Returns(true);
            //method for string what contains only letters
            //mockValidator.Setup(x => x.IsValid(It.IsInRange("a", "z", Range.Inclusive))).Returns(true);
            //method similar to regular validations
            mockValidator.Setup(x => x.IsValid(It.IsRegex("[a-z]", System.Text.RegularExpressions.RegexOptions.None))).Returns(true);
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey);
            mockValidator.DefaultValue = DefaultValue.Mock;

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 10_000,
                Age = 42,
                FrequentFlyerNumber = "x"
            };

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }

        [Fact]
        public void ReferInvalidFrequentFlyerApplications()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(false);
            //mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey);
            mockValidator.DefaultValue = DefaultValue.Mock;

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication();
            
            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        /*[Fact]
        public void DeclineLowIncomeApplicationsOutdemo()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            bool isValid = true;

            mockValidator.Setup(x => x.IsValid(It.IsAny<string>(), out isValid));

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);
            var application = new CreditCardApplication
            {
                GrossAnnualIncome = 19_999,
                Age = 42,
                FrequentFlyerNumber = "a"
            };
            
            //act
            CreditCardApplicationDecision decision = sut.EvaluateUsingOut(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }*/

        /*[Fact]
        public void ReferWhenLicennseKeyExpired()
        {
            //arrange
            var mockValidator =new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);
            //mockValidator.Setup(x => x.LicenseKey).Returns("EXPIRED");
            mockValidator.Setup(x => x.LicenseKey).Returns(GetLicenseKeyExpiryString);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication{Age = 42};
            
            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        string GetLicenseKeyExpiryString()
        {
            //E.g. read from the vendor-supplied constant file
            return "EXPIRED";
        }*/

        [Fact]
        public void ReferWhenLicennseKeyExpired()
        {
            //arrange
            var mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

            /*var mockLicenseData =new Mock<ILicenseData>();
            mockLicenseData.Setup(x => x.LicenseKey).Returns("EXPIRED");

            var mockServiceInfo = new Mock<IServiceInformation>();
            mockServiceInfo.Setup(x => x.License).Returns(mockLicenseData.Object);

            mockValidator.Setup(x => x.ServiceInformation).Returns(mockServiceInfo.Object);*/

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("EXPIRED");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            var application = new CreditCardApplication { Age = 42 };

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        string GetLicenseKeyExpiryString()
        {
            //E.g. read from the vendor-supplied constant file
            return "EXPIRED";
        }

        [Fact]
        public void IseDetailedLoopupForOlderApplications()
        {
            //arrange
            var mockValidation = new  Mock<IFrequentFlyerNumberValidator>();

            mockValidation.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidation.SetupProperty(x => x.ValidationMode);
            //or
            //mockValidation.SetupAllProperties();

            var sut = new CreditCardApplicationEvaluator(mockValidation.Object);

            var application = new CreditCardApplication{Age = 30};

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(ValidationMode.Detailed, mockValidation.Object.ValidationMode);
        }
    }
}
