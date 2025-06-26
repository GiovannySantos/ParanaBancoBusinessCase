using System.ComponentModel.DataAnnotations;

namespace CadastroClientes.Application.DTOs
{
    public class ClienteDto
    {
        [Required]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        public DateTime DataNascimento { get; set; }

        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string Telefone { get; set; } = string.Empty;
    }
}
