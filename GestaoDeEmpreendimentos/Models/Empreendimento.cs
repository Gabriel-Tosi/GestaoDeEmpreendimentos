using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeEmpreendimentos.Models
{
    public class Empreendimento
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do empreendimento (obrigatório, mínimo 3 caracteres)
        /// </summary>
        [Required]
        [MinLength(3)]
        public string Nome { get; set; } = null!;

        /// <summary>
        /// CNPJ (obrigatório e único)
        /// </summary>
        [Required]
        [Column(TypeName = "varchar(20)")]
        public string Cnpj { get; set; } = null!;

        /// <summary>
        /// Endereço do empreendimento (opcional)
        /// </summary>
        public string? Endereco { get; set; }

        /// <summary>
        /// Indica se o empreendimento está ativo (soft-delete)
        /// </summary>
        public bool Ativo { get; set; } = true;

        /// <summary>
        /// Data de criação em UTC
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
