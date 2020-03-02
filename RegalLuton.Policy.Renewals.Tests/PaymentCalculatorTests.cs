using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegalLuton.Policy.Renewals.Processor.Calculators;
using RegalLuton.Policy.Renewals.Processor.Interfaces;

namespace RegalLuton.Policy.Renewals.Tests
{
    [TestClass]
    public class PaymentCalculatorTests
    {
        ICalculator calculator;

        [TestInitialize]
        public void Setup()
        {
            calculator = new PaymentCalculator();
        }
        [TestMethod]
        public void ShouldCalculateCreditCharge()
        {
            decimal annualPremium = 50.00m;
            int percentFactor = 5;

            decimal creditCharge = calculator.CalculateCreditCharge(annualPremium, percentFactor);

            Assert.AreEqual(creditCharge, 2.50m);
        }

        [TestMethod]
        public void ShouldCalculateTotalPremium()
        {
            decimal annualPremium = 50.00m;
            decimal creditCharge = 2.50m;

            decimal totalPremium = calculator.CalculateTotalPremium(annualPremium, creditCharge);

            Assert.AreEqual(totalPremium, 52.50m);
        }

        [TestMethod]
        public void ShouldCalculateAverageMonthlyPremium()
        {
            decimal totalPremium = 52.50m;

            decimal averageMonthlyPremium = calculator.CalculateAvergeMonthlyPremium(totalPremium);

            Assert.AreEqual(averageMonthlyPremium, 4.375m);
        }

        [TestMethod]
        public void ShouldCalculateInitialMonthlyPayment()
        {
            decimal totalPremium = 52.50m;
            decimal averageMonthlyPremium = 4.375m;

            decimal initialMonthlyPayment = calculator.CalculateInitialMonthlyPaymentAmount(totalPremium, averageMonthlyPremium);

            Assert.AreEqual(initialMonthlyPayment, 4.43m);
        }

        [TestMethod]
        public void ShouldCalculateOtherMonthlyPayments()
        {
            decimal totalPremium = 52.50m;
            decimal averageMonthlyPremium = 4.375m;
            decimal initialMonthlyPayment = 4.43m;

            decimal otherMonthlyAmounts = calculator.CalculateOtherMonthlyPayments(totalPremium, averageMonthlyPremium, initialMonthlyPayment);

            Assert.AreEqual(otherMonthlyAmounts, 4.37m);
        }
    }
}
