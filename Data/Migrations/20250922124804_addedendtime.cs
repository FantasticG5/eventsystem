using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Data.Migrations
{
    /// <inheritdoc />
    public partial class addedendtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TrainingClasses",
                keyColumn: "Id",
                keyValue: 1,
                column: "EndTime",
                value: new DateTime(2025, 9, 20, 18, 3, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "TrainingClasses",
                keyColumn: "Id",
                keyValue: 2,
                column: "EndTime",
                value: new DateTime(2025, 9, 20, 18, 3, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TrainingClasses",
                keyColumn: "Id",
                keyValue: 1,
                column: "EndTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "TrainingClasses",
                keyColumn: "Id",
                keyValue: 2,
                column: "EndTime",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
