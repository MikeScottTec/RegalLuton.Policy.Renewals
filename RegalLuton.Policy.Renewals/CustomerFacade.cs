using System.Collections.Generic;
using RegalLuton.Policy.Renewals.Entities.Classes;
using RegalLuton.Policy.Renewals.Processor.Interfaces;

namespace RegalLuton.Policy.Renewals
{
    public class CustomerFacade
    {
        ICustomerProcessor customerProcessor;
        public CustomerFacade(ICustomerProcessor _customerProcessor)
        {
            customerProcessor = _customerProcessor;
        }

        public void Process(string sourceFile, string targetFolder)
        {
            List<string> rawCustomers = customerProcessor.GetCustomers(sourceFile);

            if (rawCustomers != null)
            {
                List<Customer> customers = customerProcessor.ParseCustomers(rawCustomers);
                customers = customerProcessor.CalculatePayments(customers);
                customerProcessor.GenerateCustomerLetters(customers, targetFolder);
            }
        }
    }
}
