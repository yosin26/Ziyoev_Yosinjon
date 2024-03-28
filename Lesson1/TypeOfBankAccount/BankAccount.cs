using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson1.TypeOfBankAccount
{
   abstract class BankAccount {
        private static int _accountNumberSeed = 1000000000;
        private readonly decimal _minimumBalance;
        private List<Transaction> _allTransactions = new List<Transaction>();
        // Публичное автоматическое свойство
        // Тип строка имя Number,
        // Можем только получить номер счета, но не можем установить
        public string Number { get; }

        // Баланс кошелька
        public decimal Balance { get
            {
                decimal balance = 0m;
                foreach (var item in _allTransactions)
                    balance += item.Amount;
                return balance;
            }
        }

        public string Owner { get; set; }
        public DateTime Date { get; }

        // Конструктор класса
        /// <summary>
        /// Открытие нового счета
        /// </summary>
        /// <param name="name">Имя владельца счета</param>
        /// <param name="initalBalance">Начальный баланс, по умолчанию 0</param>
        public BankAccount(string name, decimal initalBalance = 0m):this(name, 0,initalBalance)
        {
        }
        public BankAccount(string name, decimal minimumBalance, decimal initalBalance = 0m)
        {
            //this.Owner = name;
            Owner = name;
            if (initalBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(initalBalance), "Balance must be positive");
            MakeDeposit(initalBalance, DateTime.Now, "Initial balance");
            //Number = Convert.ToString(_accountNumberSeed);
            //Number = (_accountNumberSeed++).ToString();
            Number = _accountNumberSeed.ToString();
            _accountNumberSeed++;
            if (minimumBalance < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumBalance), "Balance must be positive");
            _minimumBalance = minimumBalance;

        }

            public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException
                    (nameof(amount), "Amount of deposit must be positive");
            var deposit = new Transaction(amount, date, note);
            _allTransactions.Add(deposit);
        }
        public void MakeWithdraw(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }

            Transaction? overdraftTransaction = CheckWithdrawalLimit(Balance - amount < -_minimumBalance);
            _allTransactions.Add(new(-amount, date, note));
            if (overdraftTransaction != null)
                _allTransactions.Add(overdraftTransaction);
        }

        protected virtual Transaction? CheckWithdrawalLimit(bool isOverdrawn)
        {
            if (isOverdrawn)
            {
                throw new InvalidOperationException("Not sufficient funds for this withdrawal");
            }
            else
            {
                return default;
            }
        }

        public string GetAccountHistory()
        {
            var report = new StringBuilder();
            decimal balance = 0m;
            report.AppendLine("Date\t\tAmount\tBalance\tNote");
            foreach (var item in _allTransactions)
            {
                balance += item.Amount;
                report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
            }
            return report.ToString();
        }
        public abstract void PerformMonthEndTransactions();

    }
}
