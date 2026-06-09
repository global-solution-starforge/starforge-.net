using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StarForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMissaoId1ShadowKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_FASE_MISSAO_TB_MISSAO_MissaoId1",
                table: "TB_FASE_MISSAO");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_NAVE_TB_MISSAO_MissaoId1",
                table: "TB_NAVE");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_TIER_TB_MISSAO_MissaoId1",
                table: "TB_TIER");

            migrationBuilder.DropIndex(
                name: "IX_TB_TIER_MissaoId1",
                table: "TB_TIER");

            migrationBuilder.DropIndex(
                name: "IX_TB_NAVE_MissaoId1",
                table: "TB_NAVE");

            migrationBuilder.DropIndex(
                name: "IX_TB_FASE_MISSAO_MissaoId1",
                table: "TB_FASE_MISSAO");

            migrationBuilder.DropColumn(
                name: "MissaoId1",
                table: "TB_TIER");

            migrationBuilder.DropColumn(
                name: "MissaoId1",
                table: "TB_NAVE");

            migrationBuilder.DropColumn(
                name: "MissaoId1",
                table: "TB_FASE_MISSAO");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MissaoId1",
                table: "TB_TIER",
                type: "CHAR(36)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MissaoId1",
                table: "TB_NAVE",
                type: "CHAR(36)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MissaoId1",
                table: "TB_FASE_MISSAO",
                type: "CHAR(36)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TB_TIER_MissaoId1",
                table: "TB_TIER",
                column: "MissaoId1");

            migrationBuilder.CreateIndex(
                name: "IX_TB_NAVE_MissaoId1",
                table: "TB_NAVE",
                column: "MissaoId1");

            migrationBuilder.CreateIndex(
                name: "IX_TB_FASE_MISSAO_MissaoId1",
                table: "TB_FASE_MISSAO",
                column: "MissaoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_FASE_MISSAO_TB_MISSAO_MissaoId1",
                table: "TB_FASE_MISSAO",
                column: "MissaoId1",
                principalTable: "TB_MISSAO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_NAVE_TB_MISSAO_MissaoId1",
                table: "TB_NAVE",
                column: "MissaoId1",
                principalTable: "TB_MISSAO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_TIER_TB_MISSAO_MissaoId1",
                table: "TB_TIER",
                column: "MissaoId1",
                principalTable: "TB_MISSAO",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
