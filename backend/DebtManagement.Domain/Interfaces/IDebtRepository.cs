using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DebtManagement.Domain.Entities;

namespace DebtManagement.Domain.Interfaces
{
    public interface IDebtRepository
    {
        Task<Debt?> GetByIdAsync(Guid id);
        Task<IEnumerable<Debt>> GetAllAsync();
        Task AddAsync(Debt debt);
        Task UpdateAsync(Debt debt);
        Task DeleteAsync(Guid id);
    }
}