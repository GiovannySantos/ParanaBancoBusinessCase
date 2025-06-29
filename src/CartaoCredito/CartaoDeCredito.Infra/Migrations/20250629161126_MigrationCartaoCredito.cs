using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CartaoDeCredito.Infra.Migrations
{
    /// <inheritdoc />
    public partial class MigrationCartaoCredito : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartoesCredito",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PropostaId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uuid", nullable: false),
                    Numero = table.Column<string>(type: "character varying(25)", maxLength: 25, nullable: false),
                    NomeImpresso = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Validade = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Cvv = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    Limite = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    DataExpiracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartoesCredito", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartoesCredito");
        }
    }
}
