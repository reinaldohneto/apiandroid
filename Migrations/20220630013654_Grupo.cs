using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppAndroid.Migrations
{
    public partial class Grupo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GrupoId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Grupo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GrupoId",
                table: "AspNetUsers",
                column: "GrupoId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Grupo_GrupoId",
                table: "AspNetUsers",
                column: "GrupoId",
                principalTable: "Grupo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Grupo_GrupoId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Grupo");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GrupoId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GrupoId",
                table: "AspNetUsers");
        }
    }
}
