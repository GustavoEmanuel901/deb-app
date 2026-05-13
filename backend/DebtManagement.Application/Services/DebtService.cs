using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DebtManagement.Domain.Entities;
using DebtManagement.Domain.Interfaces;
using DebtManagement.Application.DTOs;

namespace DebtManagement.Application.Services
{
    public interface IDebtService
    {
        Task<DebtResponseDto> CreateDebtAsync(CreateDebtDto createDebtDto);
        Task<IEnumerable<DebtResponseDto>> GetAllDebtsAsync();
        Task<DebtResponseDto?> GetDebtByIdAsync(Guid id);
    }

    public class DebtService : IDebtService
    {
        private readonly IDebtRepository _debtRepository;
        private readonly IMapper _mapper;

        public DebtService(IDebtRepository debtRepository, IMapper mapper)
        {
            _debtRepository = debtRepository;
            _mapper = mapper;
        }

        public async Task<DebtResponseDto> CreateDebtAsync(CreateDebtDto createDebtDto)
        {
            var debt = new Debt(
                createDebtDto.TitleNumber,
                createDebtDto.DebtorName,
                createDebtDto.DebtorCpf,
                createDebtDto.InterestRate,
                createDebtDto.FineRate
            );

            foreach (var installmentDto in createDebtDto.Installments)
            {
                debt.AddInstallment(
                    installmentDto.Number,
                    installmentDto.DueDate,
                    installmentDto.Amount
                );
            }

            await _debtRepository.AddAsync(debt);
            return await MapToResponseDto(debt);
        }

        public async Task<IEnumerable<DebtResponseDto>> GetAllDebtsAsync()
        {
            var debts = await _debtRepository.GetAllAsync();
            var responseList = new List<DebtResponseDto>();
            
            foreach (var debt in debts)
            {
                responseList.Add(await MapToResponseDto(debt));
            }
            
            return responseList;
        }

        public async Task<DebtResponseDto?> GetDebtByIdAsync(Guid id)
        {
            var debt = await _debtRepository.GetByIdAsync(id);
            if (debt == null)
                return null;
            
            return await MapToResponseDto(debt);
        }

        private async Task<DebtResponseDto> MapToResponseDto(Debt debt)
        {
            var calculation = debt.CalculateUpdate(DateTime.Today);
            
            return new DebtResponseDto
            {
                Id = debt.Id,
                TitleNumber = debt.TitleNumber,
                DebtorName = debt.DebtorName,
                DebtorCpf = debt.DebtorCpf,
                InstallmentsCount = debt.Installments.Count,
                OriginalTotal = debt.GetOriginalTotal(),
                DaysLate = calculation.DaysLate,
                UpdatedTotal = calculation.UpdatedTotal,
                Installments = _mapper.Map<List<InstallmentDetailDto>>(debt.Installments)
            };
        }
    }
}