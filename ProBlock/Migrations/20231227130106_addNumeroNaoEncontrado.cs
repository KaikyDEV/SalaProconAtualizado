using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProBlock.Migrations
{
    /// <inheritdoc />
    public partial class addNumeroNaoEncontrado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "releasedNumbers");

            migrationBuilder.CreateTable(
                name: "numeroNaoEncontrados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioResultadoPesquisa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Detalhes = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_numeroNaoEncontrados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_numeroNaoEncontrados_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_numeroNaoEncontrados_UsuarioId",
                table: "numeroNaoEncontrados",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "numeroNaoEncontrados");

            migrationBuilder.CreateTable(
                name: "releasedNumbers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Detalhes = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Numero = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    UsuarioResultadoPesquisa = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_releasedNumbers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_releasedNumbers_AspNetUsers_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_releasedNumbers_UsuarioId",
                table: "releasedNumbers",
                column: "UsuarioId");
        }
    }
}
