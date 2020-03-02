using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegalLuton.Policy.Renewals.Processor.Interfaces;
using RegalLuton.Policy.Renewals.Processor.Implementation;
using RegalLuton.Policy.Renewals.FileService.Interfaces;
using RegalLuton.Policy.Renewals.FileService.Implementation;
using RegalLuton.Policy.Renewals.Processor.Calculators;
using RegalLuton.Policy.Renewals.Entities.Enums;

namespace RegalLuton.Policy.Renewals.Tests
{
    /// <summary>
    /// Summary description for CustomerProcessorTests
    /// </summary>
    [TestClass]
    public class CustomerProcessorTests
    {
        ICustomerProcessor customerProcessor;
        IFileHandler fileHandler;
        ICalculator paymentCalculator;

        [TestInitialize]
        public void Initialise()
        {
            fileHandler = new FileHandler();
            paymentCalculator = new PaymentCalculator();
            customerProcessor = new CustomerProcessor(fileHandler, paymentCalculator);

        }
        [TestMethod]
        public void ParseCustomers_ShouldParseAllSuccessfully()
        {
            string[] data =
            {
                "ID,Title,FirstName,Surname,ProductName,PayoutAmount,AnnualPremium",
                "1,Miss,Sally,Smith,Standard Cover,190820.00,123.45",
                "2,Mr,John,Smith,Enhanced Cover,83205.50,120.00",
                "3,Mrs,Helen,Daniels,Special Cover,200000.99,141.20"
            };
            List<string> customers = new List<string>(data);

            var parsedCustomers = customerProcessor.ParseCustomers(customers);

            Assert.IsTrue(parsedCustomers.Count == 3);
            Assert.AreEqual(parsedCustomers[0].ID, 1);
            Assert.AreEqual(parsedCustomers[0].Title, Title.Miss);
            Assert.AreEqual(parsedCustomers[0].FirstName, "Sally");
            Assert.AreEqual(parsedCustomers[0].Surname, "Smith");
            Assert.AreEqual(parsedCustomers[0].ProductName, "Standard Cover");
            Assert.AreEqual(parsedCustomers[0].PayoutAmount, 190820.00m);
            Assert.AreEqual(parsedCustomers[0].AnnualPremium, 123.45m);
        }

        [TestMethod]
        public void ParseCustomers_ShouldNotParseInvalidIDRecord()
        {
            string[] data =
{
                "ID,Title,FirstName,Surname,ProductName,PayoutAmount,AnnualPremium",
                "Z,Miss,Sally,Smith,Standard Cover,190820.00,123.45",
                "2,Mr,John,Smith,Enhanced Cover,83205.50,120.00",
                "3,Mrs,Helen,Daniels,Special Cover,200000.99,141.20"
            };
            List<string> customers = new List<string>(data);

            var parsedCustomers = customerProcessor.ParseCustomers(customers);

            Assert.IsTrue(parsedCustomers.Count == 2);
        }

        [TestMethod]
        public void ParseCustomers_ShouldNotParseInvalidTitleRecord()
        {
            string[] data =
{
                "ID,Title,FirstName,Surname,ProductName,PayoutAmount,AnnualPremium",
                "1,Dr,Sally,Smith,Standard Cover,190820.00,123.45",
                "2,Mr,John,Smith,Enhanced Cover,83205.50,120.00",
                "3,Mrs,Helen,Daniels,Special Cover,200000.99,141.20"
            };
            List<string> customers = new List<string>(data);

            var parsedCustomers = customerProcessor.ParseCustomers(customers);

            Assert.IsTrue(parsedCustomers.Count == 2);
        }

        [TestMethod]
        public void ParseCustomers_ShouldNotParseInvalidPaymentAmountRecord()
        {
            string[] data =
{
                "ID,Title,FirstName,Surname,ProductName,PayoutAmount,AnnualPremium",
                "1,Miss,Sally,Smith,Standard Cover,190820.00,123.45",
                "2,Mr,John,Smith,Enhanced Cover,X83205.50,120.00",
                "3,Mrs,Helen,Daniels,Special Cover,200000.99,141.20"
            };
            List<string> customers = new List<string>(data);

            var parsedCustomers = customerProcessor.ParseCustomers(customers);

            Assert.IsTrue(parsedCustomers.Count == 2);
        }

        [TestMethod]
        public void ParseCustomers_ShouldNotParseInvalidAnnualPremiumRecord()
        {
            string[] data =
{
                "ID,Title,FirstName,Surname,ProductName,PayoutAmount,AnnualPremium",
                "1,Miss,Sally,Smith,Standard Cover,190820.00,123.45",
                "2,Mr,John,Smith,Enhanced Cover,83205.50,120.00",
                "3,Mrs,Helen,Daniels,Special Cover,200000.99,141.20xyz"
            };
            List<string> customers = new List<string>(data);

            var parsedCustomers = customerProcessor.ParseCustomers(customers);

            Assert.IsTrue(parsedCustomers.Count == 2);
        }

        [TestMethod]
        public void ParseCustomers_ShouldNotParseWithIncorrectNumberOfFields()
        {
            string[] data =
{
                "ID,Title,FirstName,Surname,ProductName,PayoutAmount,AnnualPremium",
                "1,Miss,Sally,Smith,Standard Cover,190820.00,123.45",
                "2,Mr,John,Smith,Enhanced Cover,83205.50,120.00",
                "3,Mrs,Helen,Daniels,Special Cover,200000.99,141.20",
                "4,Mr,Fred,Special Cover,200000.99,141.20"
            };
            List<string> customers = new List<string>(data);

            var parsedCustomers = customerProcessor.ParseCustomers(customers);

            Assert.IsTrue(parsedCustomers.Count == 3);
        }
    }
}
