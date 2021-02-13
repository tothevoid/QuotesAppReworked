using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuotesExchangeApp.Data.Migrations
{
    public partial class added_supportedcompany2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportedCompanies",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CompanyId = table.Column<Guid>(nullable: true),
                    SourceId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupportedCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupportedCompanies_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
               name: "DeviceCodes",
               columns: table => new
               {
                   UserCode = table.Column<string>(maxLength: 200, nullable: false),
                   DeviceCode = table.Column<string>(maxLength: 200, nullable: false),
                   SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                   ClientId = table.Column<string>(maxLength: 200, nullable: false),
                   CreationTime = table.Column<DateTime>(nullable: false),
                   Expiration = table.Column<DateTime>(nullable: false),
                   Data = table.Column<string>(maxLength: 50000, nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
               });

            migrationBuilder.CreateTable(
                name: "PersistedGrants",
                columns: table => new
                {
                    Key = table.Column<string>(maxLength: 200, nullable: false),
                    Type = table.Column<string>(maxLength: 50, nullable: false),
                    SubjectId = table.Column<string>(maxLength: 200, nullable: true),
                    ClientId = table.Column<string>(maxLength: 200, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Expiration = table.Column<DateTime>(nullable: true),
                    Data = table.Column<string>(maxLength: 50000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersistedGrants", x => x.Key);
                });


            migrationBuilder.CreateIndex(
                name: "IX_SupportedCompanies_CompanyId",
                table: "SupportedCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportedCompanies_SourceId",
                table: "SupportedCompanies",
                column: "SourceId");

            migrationBuilder.CreateIndex(
               name: "IX_DeviceCodes_DeviceCode",
               table: "DeviceCodes",
               column: "DeviceCode",
               unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DeviceCodes_Expiration",
                table: "DeviceCodes",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_Expiration",
                table: "PersistedGrants",
                column: "Expiration");

            migrationBuilder.CreateIndex(
                name: "IX_PersistedGrants_SubjectId_ClientId_Type",
                table: "PersistedGrants",
                columns: new[] { "SubjectId", "ClientId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupportedCompanies");

            migrationBuilder.DropTable(
              name: "DeviceCodes");

            migrationBuilder.DropTable(
                name: "PersistedGrants");

        }
    }
}
