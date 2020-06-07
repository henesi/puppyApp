using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AnimalDistributorService.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalType",
                columns: table => new
                {
                    AnimalTypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalType", x => x.AnimalTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Animal",
                columns: table => new
                {
                    AnimalId = table.Column<Guid>(nullable: false),
                    Alias = table.Column<string>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: false),
                    AnimalTypeRef = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animal", x => x.AnimalId);
                    table.ForeignKey(
                        name: "FK_Animal_AnimalType_AnimalTypeRef",
                        column: x => x.AnimalTypeRef,
                        principalTable: "AnimalType",
                        principalColumn: "AnimalTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Localization",
                columns: table => new
                {
                    LocalizationId = table.Column<Guid>(nullable: false),
                    Country = table.Column<string>(nullable: false),
                    City = table.Column<string>(nullable: false),
                    Street = table.Column<string>(nullable: false),
                    AnimalRef = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localization", x => x.LocalizationId);
                    table.ForeignKey(
                        name: "FK_Localization_Animal_AnimalRef",
                        column: x => x.AnimalRef,
                        principalTable: "Animal",
                        principalColumn: "AnimalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Media",
                columns: table => new
                {
                    MediaId = table.Column<Guid>(nullable: false),
                    Caption = table.Column<string>(maxLength: 450, nullable: true),
                    FileSize = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(maxLength: 450, nullable: true),
                    MediaType = table.Column<int>(nullable: false),
                    AnimalRef = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Media", x => x.MediaId);
                    table.ForeignKey(
                        name: "FK_Media_Animal_AnimalRef",
                        column: x => x.AnimalRef,
                        principalTable: "Animal",
                        principalColumn: "AnimalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(nullable: false),
                    Caption = table.Column<string>(maxLength: 450, nullable: true),
                    FileSize = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(maxLength: 450, nullable: true),
                    MediaType = table.Column<int>(nullable: false),
                    AnimalRef = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.ProfileId);
                    table.ForeignKey(
                        name: "FK_Profile_Animal_AnimalRef",
                        column: x => x.AnimalRef,
                        principalTable: "Animal",
                        principalColumn: "AnimalId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Animal_AnimalTypeRef",
                table: "Animal",
                column: "AnimalTypeRef");

            migrationBuilder.CreateIndex(
                name: "IX_Localization_AnimalRef",
                table: "Localization",
                column: "AnimalRef",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Media_AnimalRef",
                table: "Media",
                column: "AnimalRef");

            migrationBuilder.CreateIndex(
                name: "IX_Profile_AnimalRef",
                table: "Profile",
                column: "AnimalRef",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Localization");

            migrationBuilder.DropTable(
                name: "Media");

            migrationBuilder.DropTable(
                name: "Profile");

            migrationBuilder.DropTable(
                name: "Animal");

            migrationBuilder.DropTable(
                name: "AnimalType");
        }
    }
}
