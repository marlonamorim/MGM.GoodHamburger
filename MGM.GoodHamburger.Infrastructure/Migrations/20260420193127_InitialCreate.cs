using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MGM.GoodHamburger.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SandwichId = table.Column<int>(type: "integer", nullable: true),
                    SideDishId = table.Column<int>(type: "integer", nullable: true),
                    DrinkId = table.Column<int>(type: "integer", nullable: true),
                    Subtotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Total = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_MenuItems_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_MenuItems_SandwichId",
                        column: x => x.SandwichId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_MenuItems_SideDishId",
                        column: x => x.SideDishId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "Name", "Price", "Type" },
                values: new object[,]
                {
                    { 1, "X Burger", 5.00m, 1 },
                    { 2, "X Egg", 4.50m, 1 },
                    { 3, "X Bacon", 7.00m, 1 },
                    { 4, "Batata frita", 2.00m, 2 },
                    { 5, "Refrigerante", 2.50m, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DrinkId",
                table: "Orders",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SandwichId",
                table: "Orders",
                column: "SandwichId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SideDishId",
                table: "Orders",
                column: "SideDishId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "MenuItems");
        }
    }
}
