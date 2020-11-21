using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Calca.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "accounting");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "auth",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ledgers",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ledgers_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LedgerMembers",
                schema: "accounting",
                columns: table => new
                {
                    LedgerId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerMembers", x => new { x.LedgerId, x.UserId });
                    table.ForeignKey(
                        name: "FK_LedgerMembers_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalSchema: "accounting",
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LedgerMembers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LedgerOperations",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LedgerId = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatorId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LedgerOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LedgerOperations_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalSchema: "accounting",
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LedgerOperations_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OperationMembers",
                schema: "accounting",
                columns: table => new
                {
                    OperationId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationMembers", x => new { x.OperationId, x.UserId, x.Side });
                    table.ForeignKey(
                        name: "FK_OperationMembers_LedgerOperations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "accounting",
                        principalTable: "LedgerOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OperationMembers_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "auth",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_LedgerMembers_UserId",
                schema: "accounting",
                table: "LedgerMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerOperations_CreatorId",
                schema: "accounting",
                table: "LedgerOperations",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_LedgerOperations_LedgerId",
                schema: "accounting",
                table: "LedgerOperations",
                column: "LedgerId");

            migrationBuilder.CreateIndex(
                name: "IX_Ledgers_CreatorId",
                schema: "accounting",
                table: "Ledgers",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_OperationMembers_UserId",
                schema: "accounting",
                table: "OperationMembers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LedgerMembers",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "OperationMembers",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "LedgerOperations",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Ledgers",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "auth");
        }
    }
}
