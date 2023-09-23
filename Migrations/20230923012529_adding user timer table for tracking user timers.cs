using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharkValleyServer.Migrations
{
    /// <inheritdoc />
    public partial class addingusertimertablefortrackingusertimers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTimers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatrolLogId = table.Column<int>(type: "int", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedPatrolTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedPatrol = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedPatrolTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LogOutTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTimers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTimers_PatrolLogs_PatrolLogId",
                        column: x => x.PatrolLogId,
                        principalTable: "PatrolLogs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTimers_PatrolLogId",
                table: "UserTimers",
                column: "PatrolLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTimers");
        }
    }
}
