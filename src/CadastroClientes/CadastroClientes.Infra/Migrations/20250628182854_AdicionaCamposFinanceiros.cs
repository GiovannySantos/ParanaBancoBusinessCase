using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CadastroClientes.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCamposFinanceiros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Clientes");

            migrationBuilder.AddColumn<decimal>(
                name: "RendaMensal",
                table: "Clientes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ValorCreditoDesejado",
                table: "Clientes",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RendaMensal",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "ValorCreditoDesejado",
                table: "Clientes");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Clientes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
