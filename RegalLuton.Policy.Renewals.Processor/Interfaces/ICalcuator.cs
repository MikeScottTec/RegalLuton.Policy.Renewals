using System;
using System.Collections.Generic;
using System.Text;

namespace RegalLuton.Policy.Renewals.Processor.Interfaces
{
    public interface ICalculator
    {
        decimal CalculateCreditCharge(decimal annualPremium, int percentFactor);
        decimal CalculateTotalPremium(decimal annualPremium, decimal creditCharge);
        decimal CalculateAvergeMonthlyPremium(decimal totalPremium);
        decimal CalculateInitialMonthlyPaymentAmount(decimal totalPremium, decimal avergeMonthlyPremium);
        decimal CalculateOtherMonthlyPayments(decimal totalPremium, decimal averageMonthlyPremium, decimal initialMonthlyPayment);
    }
}