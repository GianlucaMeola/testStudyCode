﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.Protected;

namespace CreditCardApplications.Tests
{
    public class CreditCardApplicationEvaluatorTest
    {
        //you can build a cotructor like this
        /*private Mock<IFrequentFlyerNumberValidator> mockValidator;
        private CreditCardApplicationEvaluator sut;

        public CreditCardApplicationEvaluatorTest()
        {
                mockValidator = new Mock<IFrequentFlyerNumberValidator>();
                mockValidator.SetupAllProperties();
                mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
                mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);

                sut = new CreditCardApplicationEvaluator(mockValidator.Object);
        }*/


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

        [Fact]
        public void ShouldValidateFrequentFlyerNumberForLowIncomeApplications()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication{FrequentFlyerNumber = "q"};
            //act
            sut.Evaluate(application);

            //assert method call
            mockValidator.Verify( x => x.IsValid(It.IsAny<string>()));           
        }

        /*
        //Just to show the custom fail test message 
        [Fact]
        public void SouldValidateFrequentFlyerNumberForLowIncomeApplications_CustomMessage()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            CreditCardApplicationEvaluator sut =new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication();

            //act
            sut.Evaluate(application);

            //assert
            mockValidator.Verify(x=> x.IsValid(It.IsNotNull<string>()), "Frequent Flyer Number Should Not Be Null");

        }*/

        [Fact]
        public void NotValidateFrequentFlyerNumberForHightIncomeApplications()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            CreditCardApplicationEvaluator sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication{GrossAnnualIncome = 100_000};

            //act
            sut.Evaluate(application);

            //assert method should not be called
            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Never);

        }

        [Fact]
        public void ValidateFrequentFlyerNumberForLowIncomeApplications()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication { FrequentFlyerNumber = "q" };
            //act
            sut.Evaluate(application);

            //assert method call just one time
            mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Exactly(1));
            //mockValidator.Verify(x => x.IsValid(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void CheckLicenseKeyForLowIncomeApplications()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication { GrossAnnualIncome = 99_000};
            //act
            sut.Evaluate(application);

            //assert that LicenseKey is called to GET the value
            mockValidator.VerifyGet(x => x.ServiceInformation.License.LicenseKey, Times.Once);
        }

        [Fact]
        public void SetDetailedLookupForOlderApplications()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication { Age = 30};
            //act
            sut.Evaluate(application);

            //assert that ValidationMode is SET to Detailed
            mockValidator.VerifySet(x => x.ValidationMode = ValidationMode.Detailed);
            //if don't care, and we just want to know that SETis called
            mockValidator.VerifySet(x => x.ValidationMode = It.IsAny<ValidationMode>(), Times.Once);
        }

        [Fact]
        public void ReferWhenFrequentFlyerNumberValidationError()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");

            //generic version
            //mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Throws<Exception>();
            //non generic version
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()))
                .Throws(new Exception("Custom message"));


            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication {Age = 42};
            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, decision);
        }

        [Fact]
        public void IncrementLookupCount()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>()))
                .Returns(true)
                .Raises(x=>x.ValidatorLookupPerformed += null,  EventArgs.Empty );

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication { FrequentFlyerNumber = "x", Age = 42 };

            //act
            sut.Evaluate(application);
            //manual version event raise
            //mockValidator.Raise(x => x.ValidatorLookupPerformed += null, EventArgs.Empty);

            //assert
            Assert.Equal(1, sut.ValidatorLookupCount);
        }

        [Fact]
        public void ReferInvalidFrequentFlyerApplications_Sequence()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidator.SetupSequence(x => x.IsValid(It.IsAny<string>()))
                .Returns(false)
                .Returns(true);

            var sut = new CreditCardApplicationEvaluator(mockValidator.Object);

            CreditCardApplication application = new CreditCardApplication{Age = 25};

            //act
            CreditCardApplicationDecision firstDecision = sut.Evaluate(application);
            CreditCardApplicationDecision secondDecision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHuman, firstDecision);
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, secondDecision);
        }

        [Fact]
        public void ReferFraudRisk()
        {
            //arrange
            Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();

            Mock<FraudLookup> mockFraudLookup = new Mock<FraudLookup>();
            //mockFraudLookup.Setup(x => x.IsFraudRisk(It.IsAny<CreditCardApplication>())).Returns(true);
            mockFraudLookup.Protected().Setup<bool>("CheckApplication", ItExpr.IsAny<CreditCardApplication>())
                .Returns(true);
            var sut = new CreditCardApplicationEvaluator(mockValidator.Object, mockFraudLookup.Object);

            CreditCardApplication application = new CreditCardApplication();

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.ReferredToHumanFraudRisk, decision);
        }

        [Fact]
        public void LinqToMocks()
        {
            //arrange
            /*Mock<IFrequentFlyerNumberValidator> mockValidator = new Mock<IFrequentFlyerNumberValidator>();
            mockValidator.Setup(x => x.ServiceInformation.License.LicenseKey).Returns("OK");
            mockValidator.Setup(x => x.IsValid(It.IsAny<string>())).Returns(true);*/

            IFrequentFlyerNumberValidator mockValidator = Mock.Of<IFrequentFlyerNumberValidator>
            (
                validator =>
                    validator.ServiceInformation.License.LicenseKey == "OK" &&
                    validator.IsValid(It.IsAny<string>()) == true
            );

            var sut = new CreditCardApplicationEvaluator(mockValidator);

            CreditCardApplication application =new CreditCardApplication{Age = 25};

            //act
            CreditCardApplicationDecision decision = sut.Evaluate(application);

            //assert
            Assert.Equal(CreditCardApplicationDecision.AutoDeclined, decision);
        }
    }
}
