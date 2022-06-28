using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAndroid.Migrations
{
    public partial class VinculoLocalizacoes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Localizacoes");
            
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Localizacoes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Localizacoes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Localizacoes_UsuarioId",
                table: "Localizacoes",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Localizacoes_AspNetUsers_UsuarioId",
                table: "Localizacoes",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Localizacoes_AspNetUsers_UsuarioId",
                table: "Localizacoes");

            migrationBuilder.DropIndex(
                name: "IX_Localizacoes_UsuarioId",
                table: "Localizacoes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Localizacoes");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Localizacoes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
