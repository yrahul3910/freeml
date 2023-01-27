using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLServer.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Cards_CardId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_CardId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Pin",
                table: "UserAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserAccounts",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Balance",
                table: "UserAccounts",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "CardId",
                table: "UserAccounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "UserAccounts",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_CardId",
                table: "UserAccounts",
                column: "CardId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Cards_CardId",
                table: "UserAccounts",
                column: "CardId",
                principalTable: "Cards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
