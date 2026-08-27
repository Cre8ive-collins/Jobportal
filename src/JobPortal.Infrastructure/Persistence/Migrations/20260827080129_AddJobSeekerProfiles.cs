using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobPortal.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddJobSeekerProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobSeekerProfiles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Skills = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    Education = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    Experience = table.Column<string>(type: "character varying(10000)", maxLength: 10000, nullable: false),
                    Headline = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CvUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    UpdatedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobSeekerProfiles", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_JobSeekerProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobSeekerProfiles");
        }
    }
}
