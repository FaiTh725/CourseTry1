using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseTry1.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNessesary : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileSheduleGroup");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "Users");

            migrationBuilder.AddColumn<long>(
                name: "ProfileId",
                table: "SheduleGroups",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SheduleGroups_ProfileId",
                table: "SheduleGroups",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SheduleGroups_Profiles_ProfileId",
                table: "SheduleGroups",
                column: "ProfileId",
                principalTable: "Profiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SheduleGroups_Profiles_ProfileId",
                table: "SheduleGroups");

            migrationBuilder.DropIndex(
                name: "IX_SheduleGroups_ProfileId",
                table: "SheduleGroups");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ProfileId",
                table: "SheduleGroups");

            migrationBuilder.AddColumn<long>(
                name: "ProfileId",
                table: "Users",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProfileSheduleGroup",
                columns: table => new
                {
                    GroupsId = table.Column<long>(type: "bigint", nullable: false),
                    ProfilesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileSheduleGroup", x => new { x.GroupsId, x.ProfilesId });
                    table.ForeignKey(
                        name: "FK_ProfileSheduleGroup_Profiles_ProfilesId",
                        column: x => x.ProfilesId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfileSheduleGroup_SheduleGroups_GroupsId",
                        column: x => x.GroupsId,
                        principalTable: "SheduleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_UserId",
                table: "Profiles",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileSheduleGroup_ProfilesId",
                table: "ProfileSheduleGroup",
                column: "ProfilesId");
        }
    }
}
