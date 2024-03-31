using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTry1.Migrations
{
    /// <inheritdoc />
    public partial class FixedDayWeek : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "DayWeeks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "GroupId",
                table: "DayWeeks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
