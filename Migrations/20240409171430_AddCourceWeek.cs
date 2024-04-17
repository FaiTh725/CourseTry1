using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTry1.Migrations
{
    /// <inheritdoc />
    public partial class AddCourceWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Cource",
                table: "SheduleGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Week",
                table: "SheduleGroups",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Cources",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cource",
                table: "SheduleGroups");

            migrationBuilder.DropColumn(
                name: "Week",
                table: "SheduleGroups");

            migrationBuilder.DropColumn(
                name: "Cources",
                table: "Profiles");
        }
    }
}
