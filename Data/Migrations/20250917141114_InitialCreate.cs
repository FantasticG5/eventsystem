using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TrainingClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Instructor = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    ReservedSeats = table.Column<int>(type: "int", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingClasses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TrainingClasses",
                columns: new[] { "Id", "Capacity", "Description", "EndTime", "Instructor", "Location", "ReservedSeats", "StartTime", "Title" },
                values: new object[,]
                {
                    { 1, 20, "Ett lugnt yogapass för alla nivåer.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Anna Svensson", "Sal A", 0, new DateTime(2025, 9, 20, 17, 0, 0, 0, DateTimeKind.Utc), "Yoga Bas" },
                    { 2, 18, "Intensivt spinningpass med hög energi.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Johan Karlsson", "Spinningsalen", 0, new DateTime(2025, 9, 20, 18, 0, 0, 0, DateTimeKind.Utc), "Spinning 45" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TrainingClasses");
        }
    }
}
