using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SharkValleyServer.Migrations
{
    /// <inheritdoc />
    public partial class addignmodelbuilderforsignatureadnsupplyrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs");

            migrationBuilder.AlterColumn<int>(
                name: "SignatureId",
                table: "SupplyLogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs",
                column: "SignatureId",
                principalTable: "Signatures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs");

            migrationBuilder.AlterColumn<int>(
                name: "SignatureId",
                table: "SupplyLogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SupplyLogs_Signatures_SignatureId",
                table: "SupplyLogs",
                column: "SignatureId",
                principalTable: "Signatures",
                principalColumn: "Id");
        }
    }
}
