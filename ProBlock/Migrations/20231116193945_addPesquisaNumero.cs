using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProBlock.Migrations
{
    /// <inheritdoc />
    public partial class addPesquisaNumero : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DadosPesquisas");

            migrationBuilder.CreateTable(
                name: "PesquisaNumeros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumeroTelefone = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    ResultadoPesquisa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Detalhes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },

                constraints: table =>
                {
                    table.PrimaryKey("PK_PesquisaNumeros", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PesquisaNumeros_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PesquisaNumeros_Numeros_NumeroTelefone",
                        column: x => x.NumeroTelefone,
                        principalTable: "Numeros",
                        principalColumn: "Telefone");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PesquisaNumeros_NumeroTelefone",
                table: "PesquisaNumeros",
                column: "NumeroTelefone");

            migrationBuilder.CreateIndex(
                name: "IX_PesquisaNumeros_UsuarioId",
                table: "PesquisaNumeros",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PesquisaNumeros");

            migrationBuilder.CreateTable(
                name: "DadosPesquisas",
                columns: table => new
                {
                    NumeroTelefone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    DataDeCadastro = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DadosPesquisas", x => x.NumeroTelefone);
                });
        }
    }
}
