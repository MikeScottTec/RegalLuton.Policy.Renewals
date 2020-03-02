using RegalLuton.Policy.Renewals.Entities.Enums;

namespace RegalLuton.Policy.Renewals.Entities.Classes
{
    public class Customer
    {
        public int ID;
        public Title Title;
        public string FirstName;
        public string Surname;
        public string ProductName;
        public decimal PayoutAmount;
        public decimal AnnualPremium;
        public PaymentAmounts PaymentAmounts;

        public string CustomerName
        {
            get => $"{Title.ToString()} {FirstName} {Surname}";
        }
    }
}