using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Currencies.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class RefactoreMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRate_Currencies_FromCurrencyID",
                table: "ExchangeRate");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRate_Currencies_ToCurrencyID",
                table: "ExchangeRate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExchangeHistories_AspNetUsers_UserID",
                table: "UserExchangeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExchangeHistories_ExchangeRate_RateID",
                table: "UserExchangeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExchangeHistories_UserCurrencyAmounts_AccountID",
                table: "UserExchangeHistories");

            migrationBuilder.DropColumn(
                name: "Adres",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserExchangeHistories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RateID",
                table: "UserExchangeHistories",
                newName: "RateId");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "UserExchangeHistories",
                newName: "AccountId");

            migrationBuilder.RenameIndex(
                name: "IX_UserExchangeHistories_UserID",
                table: "UserExchangeHistories",
                newName: "IX_UserExchangeHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserExchangeHistories_RateID",
                table: "UserExchangeHistories",
                newName: "IX_UserExchangeHistories_RateId");

            migrationBuilder.RenameIndex(
                name: "IX_UserExchangeHistories_AccountID",
                table: "UserExchangeHistories",
                newName: "IX_UserExchangeHistories_AccountId");

            migrationBuilder.RenameColumn(
                name: "ToCurrencyID",
                table: "ExchangeRate",
                newName: "ToCurrencyId");

            migrationBuilder.RenameColumn(
                name: "FromCurrencyID",
                table: "ExchangeRate",
                newName: "FromCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRate_ToCurrencyID",
                table: "ExchangeRate",
                newName: "IX_ExchangeRate_ToCurrencyId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRate_FromCurrencyID",
                table: "ExchangeRate",
                newName: "IX_ExchangeRate_FromCurrencyId");

            migrationBuilder.RenameColumn(
                name: "IDNumber",
                table: "AspNetUsers",
                newName: "IdNumber");

            migrationBuilder.RenameColumn(
                name: "IDIssueDate",
                table: "AspNetUsers",
                newName: "IdIssueDate");

            migrationBuilder.RenameColumn(
                name: "IDExpiryDate",
                table: "AspNetUsers",
                newName: "IdExpiryDate");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "679381f2-06a1-4e22-beda-179e8e9e3236",
                columns: new[] { "Address", "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "Warszawa,", "b28e21da-3daa-489c-818b-e874bf6d8961", "257dd5df-a71d-41ce-bc99-c0bc18648b0c" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRate_Currencies_FromCurrencyId",
                table: "ExchangeRate",
                column: "FromCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRate_Currencies_ToCurrencyId",
                table: "ExchangeRate",
                column: "ToCurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExchangeHistories_AspNetUsers_UserId",
                table: "UserExchangeHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExchangeHistories_ExchangeRate_RateId",
                table: "UserExchangeHistories",
                column: "RateId",
                principalTable: "ExchangeRate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExchangeHistories_UserCurrencyAmounts_AccountId",
                table: "UserExchangeHistories",
                column: "AccountId",
                principalTable: "UserCurrencyAmounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRate_Currencies_FromCurrencyId",
                table: "ExchangeRate");

            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeRate_Currencies_ToCurrencyId",
                table: "ExchangeRate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExchangeHistories_AspNetUsers_UserId",
                table: "UserExchangeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExchangeHistories_ExchangeRate_RateId",
                table: "UserExchangeHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserExchangeHistories_UserCurrencyAmounts_AccountId",
                table: "UserExchangeHistories");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "UserExchangeHistories",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "RateId",
                table: "UserExchangeHistories",
                newName: "RateID");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "UserExchangeHistories",
                newName: "AccountID");

            migrationBuilder.RenameIndex(
                name: "IX_UserExchangeHistories_UserId",
                table: "UserExchangeHistories",
                newName: "IX_UserExchangeHistories_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_UserExchangeHistories_RateId",
                table: "UserExchangeHistories",
                newName: "IX_UserExchangeHistories_RateID");

            migrationBuilder.RenameIndex(
                name: "IX_UserExchangeHistories_AccountId",
                table: "UserExchangeHistories",
                newName: "IX_UserExchangeHistories_AccountID");

            migrationBuilder.RenameColumn(
                name: "ToCurrencyId",
                table: "ExchangeRate",
                newName: "ToCurrencyID");

            migrationBuilder.RenameColumn(
                name: "FromCurrencyId",
                table: "ExchangeRate",
                newName: "FromCurrencyID");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRate_ToCurrencyId",
                table: "ExchangeRate",
                newName: "IX_ExchangeRate_ToCurrencyID");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeRate_FromCurrencyId",
                table: "ExchangeRate",
                newName: "IX_ExchangeRate_FromCurrencyID");

            migrationBuilder.RenameColumn(
                name: "IdNumber",
                table: "AspNetUsers",
                newName: "IDNumber");

            migrationBuilder.RenameColumn(
                name: "IdIssueDate",
                table: "AspNetUsers",
                newName: "IDIssueDate");

            migrationBuilder.RenameColumn(
                name: "IdExpiryDate",
                table: "AspNetUsers",
                newName: "IDExpiryDate");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Adres",
                table: "AspNetUsers",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "679381f2-06a1-4e22-beda-179e8e9e3236",
                columns: new[] { "Adres", "ConcurrencyStamp", "SecurityStamp" },
                values: new object[] { "Warszawa,", "2029654c-1be5-478c-8c99-f0903b92a68d", "f88bfd74-754e-415b-ac89-02ff3d28c3f8" });

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Roles_RoleId",
                table: "AspNetUsers",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRate_Currencies_FromCurrencyID",
                table: "ExchangeRate",
                column: "FromCurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeRate_Currencies_ToCurrencyID",
                table: "ExchangeRate",
                column: "ToCurrencyID",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExchangeHistories_AspNetUsers_UserID",
                table: "UserExchangeHistories",
                column: "UserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserExchangeHistories_ExchangeRate_RateID",
                table: "UserExchangeHistories",
                column: "RateID",
                principalTable: "ExchangeRate",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserExchangeHistories_UserCurrencyAmounts_AccountID",
                table: "UserExchangeHistories",
                column: "AccountID",
                principalTable: "UserCurrencyAmounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
