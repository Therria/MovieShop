using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class UpdatingMovieTableOnTypo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwoFactorEnanbled",
                table: "User",
                newName: "TwoFactorEnabled");

            migrationBuilder.RenameColumn(
                name: "AcceddFailedCount",
                table: "User",
                newName: "AccessFailedCount");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TwoFactorEnabled",
                table: "User",
                newName: "TwoFactorEnanbled");

            migrationBuilder.RenameColumn(
                name: "AccessFailedCount",
                table: "User",
                newName: "AcceddFailedCount");
        }
    }
}
