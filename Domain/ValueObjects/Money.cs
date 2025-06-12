using Domain.Common;
using Domain.Exceptions;
namespace Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Amount { get; }
        public string Currency { get; }

        private Money() { }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new InvalidAmountException("Amount cannot be negative.");
            }
            if (string.IsNullOrWhiteSpace(currency) || currency.Length != 3)
            {
                throw new InvalidCurrencyException("Currency must be a valid 3-letter ISO code.");
            }

            Amount = amount;
            Currency = currency.ToUpperInvariant();
        }

        public Money Add(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("Cannot add Money objects with different currencies.");
            }
            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            if (Currency != other.Currency)
            {
                throw new InvalidOperationException("Cannot subtract Money objects with different currencies.");
            }
            return new Money(Amount - other.Amount, Currency);
        }

        public override string ToString()
        {
            return $"{Amount:N2} {Currency}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
            yield return Currency;
        }
    }
}
