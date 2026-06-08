using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TB_MISSAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR2(1000)", nullable: false),
                    Meta = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    TotalArrecadado = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    DataLimite = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Status = table.Column<byte>(type: "NUMBER(2)", nullable: false),
                    ImagemUrl = table.Column<string>(type: "VARCHAR2(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_MISSAO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_USUARIO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    Email = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    Rm = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    SenhaHash = table.Column<string>(type: "VARCHAR2(255)", nullable: false),
                    Ativo = table.Column<int>(type: "NUMBER(1)", nullable: false),
                    Nivel = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    TotalContribuido = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    Role = table.Column<string>(type: "VARCHAR2(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_USUARIO", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TB_FASE_MISSAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "CHAR(36)", nullable: false),
                    MissaoId = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Titulo = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR2(500)", nullable: false),
                    Ordem = table.Column<short>(type: "NUMBER(5)", nullable: false),
                    Status = table.Column<byte>(type: "NUMBER(2)", nullable: false),
                    DataConclusao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_FASE_MISSAO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_FASE_MISSAO_TB_MISSAO_MissaoId",
                        column: x => x.MissaoId,
                        principalTable: "TB_MISSAO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_NAVE",
                columns: table => new
                {
                    Id = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR2(100)", nullable: false),
                    Modelo = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    Descricao = table.Column<string>(type: "VARCHAR2(500)", nullable: false),
                    Raridade = table.Column<string>(type: "VARCHAR2(20)", nullable: false),
                    ImagemUrl = table.Column<string>(type: "VARCHAR2(255)", nullable: true),
                    MissaoId = table.Column<string>(type: "CHAR(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_NAVE", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_NAVE_TB_MISSAO_MissaoId",
                        column: x => x.MissaoId,
                        principalTable: "TB_MISSAO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_TIER",
                columns: table => new
                {
                    Id = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Nome = table.Column<string>(type: "VARCHAR2(50)", nullable: false),
                    Valor = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    BeneficioDescricao = table.Column<string>(type: "VARCHAR2(255)", nullable: false),
                    LimiteVagas = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    VagasOcupadas = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    MissaoId = table.Column<string>(type: "CHAR(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_TIER", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_TIER_TB_MISSAO_MissaoId",
                        column: x => x.MissaoId,
                        principalTable: "TB_MISSAO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TB_HANGAR",
                columns: table => new
                {
                    Id = table.Column<string>(type: "CHAR(36)", nullable: false),
                    UsuarioId = table.Column<string>(type: "CHAR(36)", nullable: false),
                    NaveId = table.Column<string>(type: "CHAR(36)", nullable: false),
                    MissaoId = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Status = table.Column<byte>(type: "NUMBER(2)", nullable: false),
                    DataAquisicao = table.Column<DateTime>(type: "TIMESTAMP", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_HANGAR", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_HANGAR_TB_MISSAO_MissaoId",
                        column: x => x.MissaoId,
                        principalTable: "TB_MISSAO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_HANGAR_TB_NAVE_NaveId",
                        column: x => x.NaveId,
                        principalTable: "TB_NAVE",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_HANGAR_TB_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TB_CONTRIBUICAO",
                columns: table => new
                {
                    Id = table.Column<string>(type: "CHAR(36)", nullable: false),
                    UsuarioId = table.Column<string>(type: "CHAR(36)", nullable: false),
                    MissaoId = table.Column<string>(type: "CHAR(36)", nullable: false),
                    TierId = table.Column<string>(type: "CHAR(36)", nullable: false),
                    Valor = table.Column<decimal>(type: "NUMBER(18,2)", nullable: false),
                    Status = table.Column<byte>(type: "NUMBER(2)", nullable: false),
                    MetodoPagamento = table.Column<byte>(type: "NUMBER(2)", nullable: false),
                    DataContribuicao = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    DataConfirmacao = table.Column<DateTime>(type: "TIMESTAMP", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TB_CONTRIBUICAO", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TB_CONTRIBUICAO_TB_MISSAO_MissaoId",
                        column: x => x.MissaoId,
                        principalTable: "TB_MISSAO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_CONTRIBUICAO_TB_TIER_TierId",
                        column: x => x.TierId,
                        principalTable: "TB_TIER",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TB_CONTRIBUICAO_TB_USUARIO_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "TB_USUARIO",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTRIBUICAO_MissaoId",
                table: "TB_CONTRIBUICAO",
                column: "MissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTRIBUICAO_TierId",
                table: "TB_CONTRIBUICAO",
                column: "TierId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_CONTRIBUICAO_UsuarioId",
                table: "TB_CONTRIBUICAO",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_FASE_MISSAO_MissaoId",
                table: "TB_FASE_MISSAO",
                column: "MissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_HANGAR_MissaoId",
                table: "TB_HANGAR",
                column: "MissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_HANGAR_NaveId",
                table: "TB_HANGAR",
                column: "NaveId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_HANGAR_UsuarioId",
                table: "TB_HANGAR",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NAVE_MissaoId",
                table: "TB_NAVE",
                column: "MissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TIER_MissaoId",
                table: "TB_TIER",
                column: "MissaoId");

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_EMAIL",
                table: "TB_USUARIO",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_RM",
                table: "TB_USUARIO",
                column: "Rm",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TB_CONTRIBUICAO");

            migrationBuilder.DropTable(
                name: "TB_FASE_MISSAO");

            migrationBuilder.DropTable(
                name: "TB_HANGAR");

            migrationBuilder.DropTable(
                name: "TB_TIER");

            migrationBuilder.DropTable(
                name: "TB_NAVE");

            migrationBuilder.DropTable(
                name: "TB_USUARIO");

            migrationBuilder.DropTable(
                name: "TB_MISSAO");
        }
    }
}
