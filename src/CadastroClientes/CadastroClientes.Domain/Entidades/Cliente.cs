using CadastroClientes.Domain.Entidades.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CadastroClientes.Domain.Entidades
{
    public class Cliente
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Nome { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string Email { get; private set; }
        public string Telefone { get; private set; }
        public StatusCliente Status { get; private set; } = StatusCliente.Ativo;
        public DateTime DataCriacao { get; private set; } = DateTime.UtcNow;

        protected Cliente() { } // EF Core

        public Cliente(string nome, string cpf, DateTime dataNascimento, string email, string telefone)
        {
            Nome = nome;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Email = email;
            Telefone = telefone;
        }

        public void MarcarComoComErro()
        {
            Status = StatusCliente.Erro;
        }
    }
}
