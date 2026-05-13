using System;
using System.Collections.Generic;

namespace DebtManagement.Application.DTOs
{
    public class CreateDebtDto
    {
        public string TitleNumber { get; set; } = string.Empty;
        public string DebtorName { get; set; } = string.Empty;
        public string DebtorCpf { get; set; } = string.Empty;
        public decimal InterestRate { get; set; }
        public decimal FineRate { get; set; }
        public List<InstallmentDto> Installments { get; set; } = new();
    }

    public class InstallmentDto
    {
        public int Number { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
    }

    public class DebtResponseDto
    {
        public Guid Id { get; set; }
        public string TitleNumber { get; set; } = string.Empty;
        public string DebtorName { get; set; } = string.Empty;
        public string DebtorCpf { get; set; } = string.Empty;
        public int InstallmentsCount { get; set; }
        public decimal OriginalTotal { get; set; }
        public int DaysLate { get; set; }
        public decimal UpdatedTotal { get; set; }
        public List<InstallmentDetailDto> Installments { get; set; } = new();
    }

    public class InstallmentDetailDto
    {
        public int Number { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        public decimal Fine { get; set; }
        public decimal Interest { get; set; }
        public decimal UpdatedAmount { get; set; }
    }
}