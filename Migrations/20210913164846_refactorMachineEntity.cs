using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LocalSNMP.Migrations
{
    public partial class refactorMachineEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SystemUptime",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<string>(
                name: "StorageFree",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StorageUsed",
                table: "Machines",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorageFree",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "StorageUsed",
                table: "Machines");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SystemUptime",
                table: "Machines",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
