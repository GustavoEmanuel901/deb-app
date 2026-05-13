namespace DebtManagement.Domain.Entities
{
    public class DebtCalculation
    {
        public decimal FineTotal { get; set; }
        public decimal InterestTotal { get; set; }
        public decimal UpdatedTotal { get; set; }
        public int DaysLate { get; set; }
    }
}