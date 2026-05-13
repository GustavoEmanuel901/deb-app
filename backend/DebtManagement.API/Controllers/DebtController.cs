using Microsoft.AspNetCore.Mvc;
using DebtManagement.Application.DTOs;
using DebtManagement.Application.Services;

namespace DebtManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DebtsController : ControllerBase
    {
        private readonly IDebtService _debtService;

        public DebtsController(IDebtService debtService)
        {
            _debtService = debtService;
        }

        /// <summary>
        /// Obtém todos os débitos
        /// </summary>
        /// <returns>Lista de débitos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DebtResponseDto>>> GetAll()
        {
            var debts = await _debtService.GetAllDebtsAsync();
            return Ok(debts);
        }

        /// <summary>
        /// Obtém um débito específico pelo ID
        /// </summary>
        /// <param name="id">ID do débito</param>
        /// <returns>Dados do débito</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<DebtResponseDto>> GetById(Guid id)
        {
            var debt = await _debtService.GetDebtByIdAsync(id);
            if (debt == null)
                return NotFound(new { message = $"Débito com ID {id} não encontrado" });

            return Ok(debt);
        }

        /// <summary>
        /// Cria um novo débito
        /// </summary>
        /// <param name="createDebtDto">Dados do débito a criar</param>
        /// <returns>Débito criado</returns>
        [HttpPost]
        public async Task<ActionResult<DebtResponseDto>> Create([FromBody] CreateDebtDto createDebtDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var debt = await _debtService.CreateDebtAsync(createDebtDto);
            return CreatedAtAction(nameof(GetById), new { id = debt.Id }, debt);
        }
    }
}
