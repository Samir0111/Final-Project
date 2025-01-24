using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinalMvc.Migrations
{
    public partial class AddedIsmainToAppointmentAndAboutSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "AppointmentSections",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "AboutSections",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "AppointmentSections");

            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "AboutSections");
        }
    }
}
