using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Satoshi.Migrations
{
    public partial class inits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProductName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: true),
                    Price = table.Column<float>(type: "REAL", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<float>(type: "REAL", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Updated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreated", "Price", "ProductName", "Updated" },
                values: new object[] { 1, new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1670), 900f, "Laptop", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreated", "Price", "ProductName", "Updated" },
                values: new object[] { 2, new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1672), 35f, "Keyboard", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "DateCreated", "Price", "ProductName", "Updated" },
                values: new object[] { 3, new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1672), 5f, "Paper", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerName", "DateCreated", "Price", "ProductId", "Updated" },
                values: new object[] { 1, "Dave", new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1766), 900f, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerName", "DateCreated", "Price", "ProductId", "Updated" },
                values: new object[] { 2, "George", new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1768), 35f, 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerName", "DateCreated", "Price", "ProductId", "Updated" },
                values: new object[] { 3, "Fiona", new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1768), 5f, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerName", "DateCreated", "Price", "ProductId", "Updated" },
                values: new object[] { 4, "Rory", new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1769), 3f, 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CustomerName", "DateCreated", "Price", "ProductId", "Updated" },
                values: new object[] { 5, "Olivia", new DateTime(2021, 12, 6, 23, 28, 1, 873, DateTimeKind.Utc).AddTicks(1769), 600f, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
