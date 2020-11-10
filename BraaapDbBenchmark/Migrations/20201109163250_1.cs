using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BraaapDbBenchmark.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Checkpoints",
                columns: table => new
                {
                    CheckpointId = table.Column<Guid>(nullable: false),
                    SessionId = table.Column<Guid>(nullable: true),
                    RiderId = table.Column<Guid>(nullable: true),
                    Timestamp = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checkpoints", x => x.CheckpointId);
                });

            migrationBuilder.CreateTable(
                name: "Riders",
                columns: table => new
                {
                    RiderId = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Riders", x => x.RiderId);
                });

            migrationBuilder.CreateTable(
                name: "RiderSessionResults",
                columns: table => new
                {
                    RiderSessionResultId = table.Column<Guid>(nullable: false),
                    SessionId = table.Column<Guid>(nullable: true),
                    RiderId = table.Column<Guid>(nullable: true),
                    RiderName = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    RiderNumber = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiderSessionResults", x => x.RiderSessionResultId);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checkpoints");

            migrationBuilder.DropTable(
                name: "Riders");

            migrationBuilder.DropTable(
                name: "RiderSessionResults");

            migrationBuilder.DropTable(
                name: "Sessions");
        }
    }
}
