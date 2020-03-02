using System;
using System.Collections.Generic;
using System.Text;
using RegalLuton.Policy.Renewals.Entities.Classes;

namespace RegalLuton.Policy.Renewals.Processor.Interfaces
{
    public interface ICustomerProcessor
    {
        List<string> GetCustomers(string sourceFile);
        List<Customer> ParseCustomers(List<string> data);
        List<Customer> CalculatePayments(List<Customer> customers);
        void GenerateCustomerLetters(List<Customer> customers, string destinationFolder);
    }
}