using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentProj.API.Migrations
{
    /// <inheritdoc />
    public partial class FixRefreshTokenTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefereshTokenExpiryTime",
                table: "Student",
                newName: "RefreshTokenExpiryTime");

            migrationBuilder.RenameColumn(
                name: "RefereshToken",
                table: "Student",
                newName: "RefreshToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiryTime",
                table: "Student",
                newName: "RefereshTokenExpiryTime");

            migrationBuilder.RenameColumn(
                name: "RefreshToken",
                table: "Student",
                newName: "RefereshToken");
        }
    }
}
