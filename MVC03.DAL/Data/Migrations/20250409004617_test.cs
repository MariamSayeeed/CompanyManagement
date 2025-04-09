using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC03.DAL.Data.Migrations
{
    /// <inheritdoc />
    public partial class test : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsArgee",
                table: "AspNetUsers",
                newName: "IsAgree");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAgree",
                table: "AspNetUsers",
                newName: "IsArgee");
        }
    }
}
