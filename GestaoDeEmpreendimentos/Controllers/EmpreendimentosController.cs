using GestaoDeEmpreendimentos.Data;
using GestaoDeEmpreendimentos.Models;
using GestaoDeEmpreendimentos.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GestaoDeEmpreendimentos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmpreendimentosController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly AutoMapper.IMapper _mapper;
        private readonly ILogger<EmpreendimentosController> _logger;

        public EmpreendimentosController(AppDbContext db, AutoMapper.IMapper mapper, ILogger<EmpreendimentosController> logger)
        {
            _db = db;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        /// <summary>
        /// Lista todos os empreendimentos. Retorna também o campo Ativo.
        /// </summary>
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Listando empreendimentos");
            try
            {
                var list = await _db.Empreendimentos.ToListAsync();
                var dto = list.Select(e => _mapper.Map<EmpreendimentoReadDto>(e));
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar empreendimentos");
                return StatusCode(500, new { message = "Erro interno ao listar empreendimentos" });
            }
        }

        [HttpGet("ativos")]
        /// <summary>
        /// Lista todos os empreendimentos ativos.
        /// </summary>
        public async Task<IActionResult> GetAtivos()
        {
            _logger.LogInformation("Listando empreendimentos ativos");
            try
            {
                var list = await _db.Empreendimentos.Where(e => e.Ativo).ToListAsync();
                var dto = list.Select(e => _mapper.Map<EmpreendimentoReadDto>(e));
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar empreendimentos ativos");
                return StatusCode(500, new { message = "Erro interno ao listar empreendimentos ativos" });
            }
        }

        [HttpGet("inativos")]
        /// <summary>
        /// Lista todos os empreendimentos inativos.
        /// </summary>
        public async Task<IActionResult> GetInativos()
        {
            _logger.LogInformation("Listando empreendimentos inativos");
            try
            {
                var list = await _db.Empreendimentos.Where(e => !e.Ativo).ToListAsync();
                var dto = list.Select(e => _mapper.Map<EmpreendimentoReadDto>(e));
                return Ok(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar empreendimentos inativos");
                return StatusCode(500, new { message = "Erro interno ao listar empreendimentos inativos" });
            }
        }

        [HttpGet("{id}")]
        /// <summary>
        /// Obtém um empreendimento pelo id.
        /// </summary>
        /// <param name="id">Identificador do empreendimento</param>
        public async Task<IActionResult> Get(int id)
        {
            _logger.LogInformation("Obtendo empreendimento id={Id}", id);
            try
            {
                var e = await _db.Empreendimentos.FindAsync(id);
                if (e == null) return NotFound(new { message = "Empreendimento não encontrado" });
                return Ok(_mapper.Map<EmpreendimentoReadDto>(e));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao obter empreendimento id={Id}", id);
                return StatusCode(500, new { message = "Erro interno ao obter empreendimento" });
            }
        }

        [HttpPost]
        /// <summary>
        /// Cria um novo empreendimento. Sempre começa como Ativo.
        /// </summary>
        /// <param name="input">Dados para criação do empreendimento</param>
        public async Task<IActionResult> Create(EmpreendimentoCreateDto input)
        {
            _logger.LogInformation("Criando empreendimento: {Nome}", input?.Nome);
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                // validações adicionais
                if (string.IsNullOrWhiteSpace(input.Cnpj))
                    return BadRequest(new { message = "CNPJ é obrigatório" });

                if (input.Nome == null || input.Nome.Trim().Length < 3)
                    return BadRequest(new { message = "Nome deve ter pelo menos 3 caracteres" });

                // check unique CNPJ
                var exists = await _db.Empreendimentos.AnyAsync(x => x.Cnpj == input.Cnpj);
                if (exists) return Conflict(new { message = "CNPJ duplicado" });

                var entity = new Empreendimento
                {
                    Nome = input.Nome.Trim(),
                    Cnpj = input.Cnpj.Trim(),
                    Endereco = input.Endereco,
                    Ativo = true,
                    CreatedAt = DateTime.UtcNow
                };

                _db.Empreendimentos.Add(entity);
                await _db.SaveChangesAsync();
                var readDto = _mapper.Map<EmpreendimentoReadDto>(entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, readDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar empreendimento");
                return StatusCode(500, new { message = "Erro interno ao criar empreendimento" });
            }
        }

        [HttpPut("{id}")]
        /// <summary>
        /// Atualiza parcialmente um empreendimento. Somente os campos enviados serão alterados.
        /// </summary>
        /// <param name="id">Identificador do empreendimento</param>
        /// <param name="input">Campos a serem atualizados</param>
        public async Task<IActionResult> Update(int id, EmpreendimentoUpdateDto input)
        {
            _logger.LogInformation("Atualizando empreendimento id={Id}", id);
            try
            {
                if (!ModelState.IsValid) return BadRequest(ModelState);

                var existing = await _db.Empreendimentos.FindAsync(id);
                if (existing == null) return NotFound(new { message = "Empreendimento não encontrado" });

                if (!existing.Ativo)
                {
                    return BadRequest(new { message = "Não é permitido editar empreendimento inativo" });
                }

                // Força que CNPJ seja único e não nulo se for enviado para atualização
                if (input.Cnpj != null && existing.Cnpj != input.Cnpj)
                {
                    var cnpjExists = await _db.Empreendimentos.AnyAsync(x => x.Cnpj == input.Cnpj && x.Id != id);
                    if (cnpjExists) return Conflict(new { message = "CNPJ duplicado" });
                    existing.Cnpj = input.Cnpj.Trim();
                }

                if (input.Nome != null)
                {
                    if (input.Nome.Trim().Length < 3) return BadRequest(new { message = "Nome deve ter pelo menos 3 caracteres" });
                    existing.Nome = input.Nome.Trim();
                }

                if (input.Endereco != null)
                {
                    existing.Endereco = input.Endereco.Trim();
                }

                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar empreendimento id={Id}", id);
                return StatusCode(500, new { message = "Erro interno ao atualizar empreendimento" });
            }
        }

        [HttpDelete("{id}")]
        /// <summary>
        /// Inativa (soft-delete) um empreendimento utilizando HTTP DELETE.
        /// </summary>
        /// <param name="id">Identificador do empreendimento</param>
        public async Task<IActionResult> Inativar(int id)
        {
            _logger.LogInformation("Inativando empreendimento id={Id}", id);
            try
            {
                var existing = await _db.Empreendimentos.FindAsync(id);
                if (existing == null) return NotFound(new { message = "Empreendimento não encontrado" });
                if (!existing.Ativo) return BadRequest(new { message = "Empreendimento já está inativo" });
                existing.Ativo = false;
                await _db.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inativar empreendimento id={Id}", id);
                return StatusCode(500, new { message = "Erro interno ao inativar empreendimento" });
            }
        }
    }
}
