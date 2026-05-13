using System;

namespace DebtManagement.Domain.Entities
{
    public class Installment
    {
        public Guid Id { get; private set; }
        public int Number { get; private set; }
        public DateTime DueDate { get; private set; }
        public decimal Amount { get; private set; }
        public Guid DebtId { get; private set; }

        public Installment(int number, DateTime dueDate, decimal amount, Guid debtId)
        {
            Id = Guid.NewGuid();
            Number = number;
            DueDate = dueDate;
            Amount = amount;
            DebtId = debtId;
        }

        // Construtor para EF Core
        private Installment() { }
    }
}