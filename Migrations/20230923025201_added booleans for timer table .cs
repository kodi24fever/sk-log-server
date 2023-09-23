using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharkValleyServer.Migrations
{
    /// <inheritdoc />
    public partial class addedbooleansfortimertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "hasEndedPatrol",
                table: "UserTimers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "hasStartedPatrol",
                table: "UserTimers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hasEndedPatrol",
                table: "UserTimers");

            migrationBuilder.DropColumn(
                name: "hasStartedPatrol",
                table: "UserTimers");
        }
    }
}
