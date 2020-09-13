using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AnimalDistributorService.Migrations
{
    public partial class AddCVTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CV_Rejection",
                columns: table => new
                {
                    RejectionId = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    MediaType = table.Column<int>(nullable: false),
                    AnimalRef = table.Column<Guid>(nullable: false),
                    Verified = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CV_Rejection", x => x.RejectionId);
                    table.ForeignKey(
                        name: "FK_CV_Rejection_Animal_AnimalRef",
                        column: x => x.AnimalRef,
                        principalTable: "Animal",
                        principalColumn: "AnimalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CV_Statistics",
                columns: table => new
                {
                    StatisticId = table.Column<Guid>(nullable: false),
                    TypeOfMedia = table.Column<int>(nullable: false),
                    ElapsedTime = table.Column<string>(nullable: true),
                    AnimalId = table.Column<Guid>(nullable: false),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CV_Statistics", x => x.StatisticId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CV_Rejection_AnimalRef",
                table: "CV_Rejection",
                column: "AnimalRef",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CV_Rejection");

            migrationBuilder.DropTable(
                name: "CV_Statistics");
        }
    }
}
