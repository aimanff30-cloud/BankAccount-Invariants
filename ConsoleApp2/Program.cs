using System;

namespace ConsoleApp2
{
    internal class BankAccount
    {
        private decimal balance;
        private decimal dailyWithdrawn;
        private const decimal DailyLimit = 3000m;
        private bool isClosed;

        public decimal Balance
        {
            get { return balance; }
        }

        public BankAccount(decimal initialBalance)
        {
            if (initialBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initialBalance), "Initial balance must be >= 0.");

            balance = initialBalance;
            isClosed = false;
            dailyWithdrawn = 0;
        }

        private void EnsureAccountIsActive()
        {
            if (isClosed)
                throw new InvalidOperationException("Account is closed.");
        }

        public void Deposit(decimal amount)
        {
            EnsureAccountIsActive();

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be > 0.");

            balance += amount;
        }

        public void Withdraw(decimal amount)
        {
            EnsureAccountIsActive();

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "Withdraw amount must be > 0.");

            if (amount > balance)
                throw new InvalidOperationException("Insufficient balance.");

            if (dailyWithdrawn + amount > DailyLimit)
                throw new InvalidOperationException("Daily withdrawal limit exceeded.");

            dailyWithdrawn += amount;
            balance -= amount;
        }

        public void TransferTo(BankAccount target, decimal amount)
        {
            EnsureAccountIsActive();

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            if (target == this)
                throw new InvalidOperationException("Cannot transfer to the same account.");

            Withdraw(amount);
            target.Deposit(amount);
        }

        public void Close()
        {
            if (isClosed)
                throw new InvalidOperationException("Account is already closed.");

            if (balance != 0)
                throw new InvalidOperationException("Cannot close account with non-zero balance.");

            isClosed = true;
        }

        public void Open()
        {
            if (!isClosed)
                throw new InvalidOperationException("Account is already open.");

            isClosed = false;
        }

        public void ResetDailyLimit()
        {
            dailyWithdrawn = 0;
        }

        public override string ToString()
        {
            return $"Balance: {balance:C}";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            BankAccount a = new BankAccount(10000);
            BankAccount b = new BankAccount(5000);

            a.Withdraw(500);
            a.TransferTo(b, 400);

            Console.WriteLine(a);
            Console.WriteLine(b);
        }
    }
}
