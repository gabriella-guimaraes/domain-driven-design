using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContainRs.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustesCurso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propostas_Solicitacoes_SolicitacaoId",
                table: "Propostas");

            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantidadeEstimada = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Finalidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicioOperacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisponibilidadePrevia = table.Column<int>(type: "int", nullable: false),
                    DuracaoPrevistaLocacao = table.Column<int>(type: "int", nullable: false),
                    EnderecoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Localizacao_CEP = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Localizacao_Referencias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Localizacao_Latitude = table.Column<double>(type: "float", nullable: true),
                    Localizacao_Longitude = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pedidos", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Propostas_Pedidos_SolicitacaoId",
                table: "Propostas",
                column: "SolicitacaoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propostas_Pedidos_SolicitacaoId",
                table: "Propostas");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EnderecoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataInicioOperacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisponibilidadePrevia = table.Column<int>(type: "int", nullable: false),
                    DuracaoPrevistaLocacao = table.Column<int>(type: "int", nullable: false),
                    Finalidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantidadeEstimada = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Endereco_EnderecoId",
                        column: x => x.EnderecoId,
                        principalTable: "Endereco",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_EnderecoId",
                table: "Solicitacoes",
                column: "EnderecoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Propostas_Solicitacoes_SolicitacaoId",
                table: "Propostas",
                column: "SolicitacaoId",
                principalTable: "Solicitacoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
