namespace GestaoDeEmpreendimentos.Dtos
{
    /// <summary>
    /// DTO de leitura retornado pelas APIs
    /// </summary>
    public class EmpreendimentoReadDto
    {
        public int Id { get; set; }
        /// <summary>Nome do empreendimento</summary>
        public string Nome { get; set; } = null!;
        /// <summary>CNPJ</summary>
        public string Cnpj { get; set; } = null!;
        /// <summary>Endereço</summary>
        public string? Endereco { get; set; }
        /// <summary>Indica se está ativo</summary>
        public bool Ativo { get; set; }
    }
}
