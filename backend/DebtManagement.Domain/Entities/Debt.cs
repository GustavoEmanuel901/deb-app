using System;
using System.Collections.Generic;
using System.Linq;

namespace DebtManagement.Domain.Entities
{
    public class Debt
    {
        public Guid Id { get; private set; }
        public string TitleNumber { get; private set; }
        public string DebtorName { get; private set; }
        public string DebtorCpf { get; private set; }
        public decimal InterestRate { get; private set; }
        public decimal FineRate { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private readonly List<Installment> _installments;
        public IReadOnlyCollection<Installment> Installments => _installments;

        public Debt(string titleNumber, string debtorName, string debtorCpf, 
                   decimal interestRate, decimal fineRate)
        {
            Id = Guid.NewGuid();
            TitleNumber = titleNumber;
            DebtorName = debtorName;
            DebtorCpf = debtorCpf;
            InterestRate = interestRate;
            FineRate = fineRate;
            CreatedAt = DateTime.UtcNow;
            _installments = new List<Installment>();
        }

        // Construtor para EF Core
        private Debt()
        {
            _installments = new List<Installment>();
        }

        public void AddInstallment(int number, DateTime dueDate, decimal amount)
        {
            var installment = new Installment(number, dueDate, amount, Id);
            _installments.Add(installment);
        }

        public decimal GetOriginalTotal()
        {
            return Installments.Sum(i => i.Amount);
        }

        public DebtCalculation CalculateUpdate(DateTime referenceDate)
        {
            var calculation = new DebtCalculation();
            decimal updatedTotal = 0;
            decimal fineTotal = 0;
            decimal interestTotal = 0;
            int maxDaysLate = 0;

            foreach (var installment in Installments)
            {
                var daysLate = (referenceDate.Date - installment.DueDate.Date).Days;
                if (daysLate <= 0) 
                {
                    updatedTotal += installment.Amount;
                    continue;
                }

                // Calcular multa
                var fine = installment.Amount * (FineRate / 100);
                
                // Calcular juros (diário)
                var monthlyInterestRate = InterestRate / 100;
                var dailyInterestRate = monthlyInterestRate / 30;
                var interest = installment.Amount * dailyInterestRate * daysLate;
                
                fineTotal += fine;
                interestTotal += interest;
                updatedTotal += installment.Amount + fine + interest;
                
                if (daysLate > maxDaysLate)
                    maxDaysLate = daysLate;
            }

            calculation.FineTotal = fineTotal;
            calculation.InterestTotal = interestTotal;
            calculation.UpdatedTotal = updatedTotal;
            calculation.DaysLate = maxDaysLate;

            return calculation;
        }
    }
}