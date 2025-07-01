using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroClientes.Infra.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClienteCartao",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    CartaoId = table.Column<Guid>(type: "uuid", nullable: false),
                    PropostaId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumeroCartao = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    NomeImpresso = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Validade = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false),
                    Limite = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteCartao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Telefone = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    RendaMensal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ValorCreditoDesejado = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventoErro",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Origem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RoutingKey = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Payload = table.Column<string>(type: "text", nullable: false),
                    Erro = table.Column<string>(type: "text", nullable: false),
                    DataOcorrencia = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoErro", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClienteCartao_CartaoId",
                table: "ClienteCartao",
                column: "CartaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClienteCartao_ClienteId",
                table: "ClienteCartao",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClienteCartao_PropostaId",
                table: "ClienteCartao",
                column: "PropostaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClienteCartao");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "EventoErro");
        }
    }
}
