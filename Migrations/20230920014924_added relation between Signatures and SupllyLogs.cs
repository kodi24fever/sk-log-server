using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharkValleyServer.Migrations
{
    /// <inheritdoc />
    public partial class addedrelationbetweenSignaturesandSupllyLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "SupplyLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SupplyLogs_SignatureId",
                table: "SupplyLogs",
                column: "SignatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs",
                column: "SignatureId",
                principalTable: "Signatures",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs");

            migrationBuilder.DropIndex(
                name: "IX_SupplyLogs_SignatureId",
                table: "SupplyLogs");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "SupplyLogs");
        }
    }
}
