using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MLServer.Infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EpochsRun",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Owner",
                table: "Jobs",
                type: "varchar",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EpochsRun",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Owner",
                table: "Jobs");
        }
    }
}
