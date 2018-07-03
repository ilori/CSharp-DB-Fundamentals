namespace P01_BillsPaymentSystem.Data.Models
{
    using System;

    public class CreditCard
    {
        public int CreditCardId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public decimal Limit { get; set; }

        public decimal MoneyOwed { get; set; }

        public decimal LimitLeft => this.Limit - this.MoneyOwed;

        public int PaymentMethodId { get; set; }
        public PaymentMethod PaymentMethod { get; set; }


        public void Withdraw(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"Please enter positive amount!");
            }
            if (amount > this.LimitLeft)
            {
                throw new ArgumentException($"Insufficient funds!");
            }
            Console.WriteLine($"You successfully payed ${amount:F2} dollars!");
            this.MoneyOwed += amount;
        }

        public void Deposit(decimal amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException($"Please enter positive amount!");
            }
            if (amount <= this.MoneyOwed)
            {
                this.MoneyOwed += amount;
            }
            else
            {
                this.Limit += amount - this.MoneyOwed;
                this.MoneyOwed = 0;
            }
        }
    }
}