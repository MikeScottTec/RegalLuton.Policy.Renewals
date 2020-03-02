using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using RegalLuton.Policy.Renewals.Entities.Classes;
using RegalLuton.Policy.Renewals.Entities.Enums;
using RegalLuton.Policy.Renewals.Processor.Interfaces;
using RegalLuton.Policy.Renewals.FileService.Interfaces;

namespace RegalLuton.Policy.Renewals.Processor.Implementation
{
    public class CustomerProcessor : ICustomerProcessor
    {
        private IFileHandler fileHandler;
        private ICalculator calculator;

        public CustomerProcessor(IFileHandler _fileHandler, ICalculator _calculator)
        {
            fileHandler = _fileHandler;
            calculator = _calculator;
        }

        /// <summary>
        /// Read contents of the customer csv file
        /// </summary>
        /// <param name="folder">Location of the customer csv file</param>
        /// <param name="fileName">Name of the customer csv file</param>
        /// <returns>List of customers or null if file does not exist</returns>
        public List<string> GetCustomers(string sourceFile)
        {
            if (fileHandler.FileExists(sourceFile))
            {
                return fileHandler.ReadAllLines(sourceFile).ToList();
            }
            return null;
        }

        /// <summary>
        /// Parse the csv customer records into a list of customer objects
        /// </summary>
        /// <param name="data">List of csv data lines</param>
        /// <returns>List of parsed customer objects</returns>
        public List<Customer> ParseCustomers(List<string> data)
        {
            List<Customer> customers = new List<Customer>();

            if (data != null)
            {
                //First element in the list is the header so ignore it
                foreach (string line in data.Skip(1))
                {
                    string[] items = line.Split(',');
                    if (items.Length == 7)
                    {
                        if (!int.TryParse(items[0], out int id))
                            continue;

                        if (!Enum.TryParse(items[1], true, out Title title))
                            continue;

                        string firstName = items[2];
                        string surname = items[3];
                        string productName = items[4];

                        if (!decimal.TryParse(items[5], out decimal paymentAmount))
                            continue;

                        if (!decimal.TryParse(items[6], out decimal annualPremium))
                            continue;

                        customers.Add(new Customer
                        {
                            ID = id,
                            Title = title,
                            FirstName = firstName,
                            Surname = surname,
                            ProductName = productName,
                            PayoutAmount = paymentAmount,
                            AnnualPremium = annualPremium,
                            PaymentAmounts = new PaymentAmounts()
                        });
                    }
                }
            }
            return customers;
        }

        /// <summary>
        /// Calculate for the Payment amounts for each customer
        /// </summary>
        /// <param name="customers">List of customers to process</param>
        /// <returns>Customers will calculated payment amunts</returns>
        public List<Customer> CalculatePayments(List<Customer> customers)
        {
            customers.ForEach(c =>
            {
                c.PaymentAmounts.CreditCharge = calculator.CalculateCreditCharge(c.AnnualPremium, 5);
                c.PaymentAmounts.TotalPremium = calculator.CalculateTotalPremium(c.AnnualPremium, c.PaymentAmounts.CreditCharge);
                c.PaymentAmounts.AverageMonthlyPremium = calculator.CalculateAvergeMonthlyPremium(c.PaymentAmounts.TotalPremium);
                c.PaymentAmounts.InitialMonthlyPaymentAmount = calculator.CalculateInitialMonthlyPaymentAmount(c.PaymentAmounts.TotalPremium,
                                                                                c.PaymentAmounts.AverageMonthlyPremium);
                c.PaymentAmounts.OtherMonthlyPaymentsAmount = calculator.CalculateOtherMonthlyPayments(c.PaymentAmounts.TotalPremium,
                                                                                c.PaymentAmounts.AverageMonthlyPremium,
                                                                                c.PaymentAmounts.InitialMonthlyPaymentAmount);
            });
            return customers;
        }

        /// <summary>
        /// Generate letters for each customer in the input list
        /// </summary>
        /// <param name="customers">List of customers to generate letters for</param>
        /// <param name="destinationFolder">The folder to write the letters to</param>
        public void GenerateCustomerLetters(List<Customer> customers, string destinationFolder)
        {
            string letterTemplate = fileHandler.GetLetterTemplate();

            customers.ForEach(c =>
            {
                string fileName = GenerateFileName(c);
                if (!fileHandler.FileExists(destinationFolder, fileName))
                {
                    string customerLetter = letterTemplate;
                    customerLetter = ReplaceText(customerLetter, "[Date]", DateTime.Now.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.CustomerName)}]", c.CustomerName);
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.Title)}]", c.Title.ToString());
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.Surname)}]", c.Surname.ToString());
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.ProductName)}]", c.ProductName);
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.PayoutAmount)}]", c.PayoutAmount.ToString("C"));
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.AnnualPremium)}]", c.AnnualPremium.ToString("C"));
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.PaymentAmounts.CreditCharge)}]", c.PaymentAmounts.CreditCharge.ToString("C"));
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.PaymentAmounts.TotalPremium)}]", c.PaymentAmounts.TotalPremium.ToString("C"));
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.PaymentAmounts.InitialMonthlyPaymentAmount)}]", c.PaymentAmounts.InitialMonthlyPaymentAmount.ToString("C"));
                    customerLetter = ReplaceText(customerLetter, $"[{nameof(c.PaymentAmounts.OtherMonthlyPaymentsAmount)}]", c.PaymentAmounts.OtherMonthlyPaymentsAmount.ToString("C"));

                    fileHandler.Write(destinationFolder, fileName, customerLetter);
                }
            });
        }

        /// <summary>
        /// Generate letter fie name for the specified customer
        /// </summary>
        /// <param name="customer">Customer record</param>
        /// <returns>Customer letter filename</returns>
        private string GenerateFileName(Customer customer)
        {
            return $"{customer.ID}_{customer.Title}{customer.FirstName}{customer.Surname}.txt";
        }

        /// <summary>
        /// Replace the specified field with the specified value in the content string
        /// </summary>
        /// <param name="content">Content string</param>
        /// <param name="fieldName">Field Name to replace</param>
        /// <param name="fieldValue">Field Value to replace</param>
        /// <returns>Updated content string</returns>
        private string ReplaceText(string content, string fieldName, string fieldValue)
        {
            return content.Replace(fieldName, fieldValue);
        }
    }
}