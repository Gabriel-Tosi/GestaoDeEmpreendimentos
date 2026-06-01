using System.ComponentModel.DataAnnotations;

namespace GestaoDeEmpreendimentos.Dtos
{
    /// <summary>
    /// DTO utilizado para atualização parcial (patch-like). Campos nulos não serão alterados.
    /// </summary>
    public class EmpreendimentoUpdateDto
    {
        /// <summary>
        /// Nome (opcional)
        /// </summary>
        [MinLength(3)]
        public string? Nome { get; set; }

        /// <summary>
        /// CNPJ (opcional)
        /// </summary>
        [MinLength(1)]
        public string? Cnpj { get; set; }

        /// <summary>
        /// Endereço (opcional)
        /// </summary>
        public string? Endereco { get; set; }
    }
}
