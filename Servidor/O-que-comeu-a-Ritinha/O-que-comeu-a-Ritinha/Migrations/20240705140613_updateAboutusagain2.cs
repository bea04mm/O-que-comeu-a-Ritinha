using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace O_que_comeu_a_Ritinha.Migrations
{
    /// <inheritdoc />
    public partial class updateAboutusagain2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LinkFacebook",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "LinkInstagram",
                table: "Aboutus");

            migrationBuilder.DropColumn(
                name: "LinkYoutube",
                table: "Aboutus");

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Utilizadores",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Utilizadores",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);

            migrationBuilder.AddColumn<string>(
                name: "LinkFacebook",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkInstagram",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LinkYoutube",
                table: "Aboutus",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
