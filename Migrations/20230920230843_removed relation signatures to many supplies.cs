using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharkValleyServer.Migrations
{
    /// <inheritdoc />
    public partial class removedrelationsignaturestomanysupplies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signatures_PatrolLogs_PatrolLogId",
                table: "Signatures");

            migrationBuilder.DropForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs");

            migrationBuilder.DropIndex(
                name: "IX_SupplyLogs_SignatureId",
                table: "SupplyLogs");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "SupplyLogs");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Signatures_PatrolLogs_PatrolLogId",
                table: "Signatures");

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "SupplyLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PatrolLogId",
                table: "Signatures",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyLogs_SignatureId",
                table: "SupplyLogs",
                column: "SignatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Signatures_PatrolLogs_PatrolLogId",
                table: "Signatures",
                column: "PatrolLogId",
                principalTable: "PatrolLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs",
                column: "SignatureId",
                principalTable: "Signatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
