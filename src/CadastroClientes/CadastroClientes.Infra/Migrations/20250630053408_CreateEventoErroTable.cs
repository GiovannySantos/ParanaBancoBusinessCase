using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroClientes.Infra.Migrations
{
    /// <inheritdoc />
    public partial class CreateEventoErroTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventoErro");
        }
    }
}
