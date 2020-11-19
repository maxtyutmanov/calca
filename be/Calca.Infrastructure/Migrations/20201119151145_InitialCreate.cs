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

            migrationBuilder.CreateTable(
                name: "Ledgers",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ledgers", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LedgerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Members_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalSchema: "accounting",
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                schema: "accounting",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LedgerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id)
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_Operations_Ledgers_LedgerId",
                        column: x => x.LedgerId,
                        principalSchema: "accounting",
                        principalTable: "Ledgers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OperationMembers",
                schema: "accounting",
                columns: table => new
                {
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    OperationId = table.Column<long>(type: "bigint", nullable: false),
                    Side = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OperationMembers", x => new { x.OperationId, x.MemberId, x.Side })
                        .Annotation("SqlServer:Clustered", true);
                    table.ForeignKey(
                        name: "FK_OperationMembers_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "accounting",
                        principalTable: "Members",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OperationMembers_Operations_OperationId",
                        column: x => x.OperationId,
                        principalSchema: "accounting",
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_LedgerId_UserId",
                schema: "accounting",
                table: "Members",
                columns: new[] { "LedgerId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OperationMembers_MemberId",
                schema: "accounting",
                table: "OperationMembers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Operations_LedgerId",
                schema: "accounting",
                table: "Operations",
                column: "LedgerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OperationMembers",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Members",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Operations",
                schema: "accounting");

            migrationBuilder.DropTable(
                name: "Ledgers",
                schema: "accounting");
        }
    }
}
