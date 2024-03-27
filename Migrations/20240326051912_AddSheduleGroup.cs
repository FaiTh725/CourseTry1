using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTry1.Migrations
{
    /// <inheritdoc />
    public partial class AddSheduleGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SheduleGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameGroup = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SheduleGroups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayWeeks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    SheduleGroupId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayWeeks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DayWeeks_SheduleGroups_SheduleGroupId",
                        column: x => x.SheduleGroupId,
                        principalTable: "SheduleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subjects",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayWeekId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subjects_DayWeeks_DayWeekId",
                        column: x => x.DayWeekId,
                        principalTable: "DayWeeks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DayWeeks_SheduleGroupId",
                table: "DayWeeks",
                column: "SheduleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Subjects_DayWeekId",
                table: "Subjects",
                column: "DayWeekId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subjects");

            migrationBuilder.DropTable(
                name: "DayWeeks");

            migrationBuilder.DropTable(
                name: "SheduleGroups");
        }
    }
}
