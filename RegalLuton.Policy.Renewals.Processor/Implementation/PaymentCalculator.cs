using RegalLuton.Policy.Renewals.Processor.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace RegalLuton.Policy.Renewals.Processor.Calculators
{
    public class PaymentCalculator : ICalculator
    {
        /// <summary>
        /// Calculate Credit Charge rounded to nearest pence
        /// </summary>
        /// <param name="annualPremium">Annual payment value</param>
        /// <param name="percentFactor">Percentage factor</param>
        /// <returns>Credit Charge value</returns>
        public decimal CalculateCreditCharge(decimal annualPremium, int percentFactor)
        {
            return Math.Round(annualPremium * (percentFactor / 100m), 2);
        }

        /// <summary>
        /// Calculate the Total Premium
        /// </summary>
        /// <param name="annualPremium">The Annual Premium</param>
        /// <param name="creditCharge">The Credit Charge</param>
        /// <returns>The total premium</returns>
        public decimal CalculateTotalPremium(decimal annualPremium, decimal creditCharge)
        {
            return annualPremium + creditCharge;
        }

        /// <summary>
        /// Calculate the averge monthly premium
        /// </summary>
        /// <param name="totalPremium">The total premium value</param>
        /// <returns>The Averge Monthly premium</returns>
        public decimal CalculateAvergeMonthlyPremium(decimal totalPremium)
        {
            return totalPremium / 12;
        }

        /// <summary>
        /// Calculate the Initial Monthly Payment
        /// </summary>
        /// <param name="totalPremium">The Total premium value</param>
        /// <param name="avergeMonthlyPremium">The average monthly premium</param>
        /// <returns>Initial Monthly premium</returns>
        public decimal CalculateInitialMonthlyPaymentAmount(decimal totalPremium, decimal avergeMonthlyPremium)
        {
            decimal truncValue = TruncateTo2Dp(avergeMonthlyPremium);
            if (avergeMonthlyPremium == truncValue)
            {
                return TruncateTo2Dp(avergeMonthlyPremium);
            }
            else
            {
                decimal diff = avergeMonthlyPremium - truncValue;
                return TruncateTo2Dp(truncValue + (diff * 12));
            }
        }

        /// <summary>
        /// Calculate the Other Monthly premium amounts
        /// </summary>
        /// <param name="totalPremium">The total premium value</param>
        /// <param name="averageMonthlyPremium">The average monthly premium</param>
        /// <param name="initialMonthlyPayment">The initial monthy premium</param>
        /// <returns>The other monthly premium value</returns>
        public decimal CalculateOtherMonthlyPayments(decimal totalPremium, decimal averageMonthlyPremium, decimal initialMonthlyPayment)
        {
            if (averageMonthlyPremium == TruncateTo2Dp(averageMonthlyPremium))
            {
                return averageMonthlyPremium;
            }
            else
            {
                return TruncateTo2Dp((totalPremium - initialMonthlyPayment) / 11m);
            }
        }

        /// <summary>
        /// Truncates a decimal number to 2 numbers after the decimal point with no rounding (for example 4.375 > 4.37)
        /// </summary>
        /// <param name="value">The value to truncate</param>
        /// <returns>Truncated value</returns>
        private decimal TruncateTo2Dp(decimal value)
        {
            return Math.Truncate(value * 100) / 100;
        }
    }
}