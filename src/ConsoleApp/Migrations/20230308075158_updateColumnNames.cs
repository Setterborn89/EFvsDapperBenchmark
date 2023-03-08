using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp.Migrations
{
    /// <inheritdoc />
    public partial class updateColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "courses",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "teachers",
                schema: "dbo");

            migrationBuilder.RenameColumn(
                name: "last_name",
                schema: "dbo",
                table: "student",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "first_name",
                schema: "dbo",
                table: "student",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "birth_date",
                schema: "dbo",
                table: "student",
                newName: "BirthDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                schema: "dbo",
                table: "student",
                newName: "last_name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                schema: "dbo",
                table: "student",
                newName: "first_name");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                schema: "dbo",
                table: "student",
                newName: "birth_date");

            migrationBuilder.CreateTable(
                name: "courses",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.Id);
                });
        }
    }
}
