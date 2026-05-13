using Microsoft.EntityFrameworkCore;
using DebtManagement.Domain.Entities;
using DebtManagement.Domain.Interfaces;
using DebtManagement.Infrastructure.Data;

namespace DebtManagement.Infrastructure.Repositories
{
    public class DebtRepository : IDebtRepository
    {
        private readonly DebtDbContext _context;

        public DebtRepository(DebtDbContext context)
        {
            _context = context;
        }

        public async Task<Debt?> GetByIdAsync(Guid id)
        {
            return await _context.Debts
                .Include(d => d.Installments)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<Debt>> GetAllAsync()
        {
            return await _context.Debts
                .Include(d => d.Installments)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task AddAsync(Debt debt)
        {
            await _context.Debts.AddAsync(debt);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Debt debt)
        {
            _context.Debts.Update(debt);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var debt = await GetByIdAsync(id);
            if (debt != null)
            {
                _context.Debts.Remove(debt);
                await _context.SaveChangesAsync();
            }
        }
    }
}