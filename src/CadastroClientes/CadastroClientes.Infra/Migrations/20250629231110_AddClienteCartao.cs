using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroClientes.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddClienteCartao : Migration
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
                    Validade = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Limite = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClienteCartao", x => x.Id);
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
        }
    }
}
