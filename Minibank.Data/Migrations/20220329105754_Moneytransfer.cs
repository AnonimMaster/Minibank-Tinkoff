using Microsoft.EntityFrameworkCore.Migrations;

namespace Minibank.Data.Migrations
{
    public partial class Moneytransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "money_transfer",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<double>(type: "double precision", nullable: false),
                    currency_code = table.Column<int>(type: "integer", nullable: false),
                    from_account_id = table.Column<string>(type: "text", nullable: false),
                    to_account_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_id", x => x.id);
                    table.ForeignKey(
                        name: "fk_money_transfer_bank_account_from_account_id",
                        column: x => x.from_account_id,
                        principalTable: "bank_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_money_transfer_bank_account_to_account_id",
                        column: x => x.to_account_id,
                        principalTable: "bank_account",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_money_transfer_from_account_id",
                table: "money_transfer",
                column: "from_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_money_transfer_to_account_id",
                table: "money_transfer",
                column: "to_account_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "money_transfer");
        }
    }
}
