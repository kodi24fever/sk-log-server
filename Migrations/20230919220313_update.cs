using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharkValleyServer.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signatures_PatrolLogs_PatrolLogId",
                table: "Signatures");

            migrationBuilder.AlterColumn<int>(
                name: "PatrolLogId",
                table: "Signatures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Signatures_PatrolLogs_PatrolLogId",
                table: "Signatures",
                column: "PatrolLogId",
                principalTable: "PatrolLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signatures_PatrolLogs_PatrolLogId",
                table: "Signatures");

            migrationBuilder.AlterColumn<int>(
                name: "PatrolLogId",
                table: "Signatures",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Signatures_PatrolLogs_PatrolLogId",
                table: "Signatures",
                column: "PatrolLogId",
                principalTable: "PatrolLogs",
                principalColumn: "Id");
        }
    }
}
