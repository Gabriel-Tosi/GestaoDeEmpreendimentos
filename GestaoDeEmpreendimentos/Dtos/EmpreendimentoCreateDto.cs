using System.ComponentModel.DataAnnotations;

namespace GestaoDeEmpreendimentos.Dtos
{
    /// <summary>
    /// DTO utilizado para criação de empreendimentos
    /// </summary>
    public class EmpreendimentoCreateDto
    {
        /// <summary>
        /// Nome do empreendimento (mínimo 3 caracteres)
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Nome { get; set; } = null!;

        /// <summary>
        /// CNPJ do empreendimento
        /// </summary>
        [Required]
        [MinLength(1)]
        public string Cnpj { get; set; } = null!;

        public string? Endereco { get; set; }
    }
}
