using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mango.Services.EmailAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddEemailServiceDb2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "EmailLoggers",
                newName: "EmailTo");

            migrationBuilder.AddColumn<string>(
                name: "EmailFrom",
                table: "EmailLoggers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailFrom",
                table: "EmailLoggers");

            migrationBuilder.RenameColumn(
                name: "EmailTo",
                table: "EmailLoggers",
                newName: "Email");
        }
    }
}
