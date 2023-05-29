using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccuFin.Api.Migrations
{
    /// <inheritdoc />
    public partial class addlinkedaccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Administrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdministrationRegistryCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TelephoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailAdress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HasImage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Administrations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizedUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailAdress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telephone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizedUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LinkBankAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdministrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IBAN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sync = table.Column<bool>(type: "bit", nullable: false),
                    LastSync = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LinkBankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LinkBankAccounts_Administrations_AdministrationId",
                        column: x => x.AdministrationId,
                        principalTable: "Administrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BankIntegrations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdministrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Institution = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitializedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExternalLinkId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accepted = table.Column<bool>(type: "bit", nullable: false),
                    AcceptedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankIntegrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankIntegrations_Administrations_AdministrationId",
                        column: x => x.AdministrationId,
                        principalTable: "Administrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BankIntegrations_AuthorizedUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthorizedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAdministrationLink",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdministrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Roles = table.Column<int>(type: "int", nullable: false),
                    AdministrationId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AuthorizedUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAdministrationLink", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAdministrationLink_Administrations_AdministrationId",
                        column: x => x.AdministrationId,
                        principalTable: "Administrations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAdministrationLink_Administrations_AdministrationId1",
                        column: x => x.AdministrationId1,
                        principalTable: "Administrations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAdministrationLink_AuthorizedUsers_AuthorizedUserId",
                        column: x => x.AuthorizedUserId,
                        principalTable: "AuthorizedUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAdministrationLink_AuthorizedUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthorizedUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankIntegrations_AdministrationId",
                table: "BankIntegrations",
                column: "AdministrationId");

            migrationBuilder.CreateIndex(
                name: "IX_BankIntegrations_UserId",
                table: "BankIntegrations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LinkBankAccounts_AdministrationId",
                table: "LinkBankAccounts",
                column: "AdministrationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdministrationLink_AdministrationId",
                table: "UserAdministrationLink",
                column: "AdministrationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdministrationLink_AdministrationId1",
                table: "UserAdministrationLink",
                column: "AdministrationId1");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdministrationLink_AuthorizedUserId",
                table: "UserAdministrationLink",
                column: "AuthorizedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAdministrationLink_UserId",
                table: "UserAdministrationLink",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankIntegrations");

            migrationBuilder.DropTable(
                name: "LinkBankAccounts");

            migrationBuilder.DropTable(
                name: "UserAdministrationLink");

            migrationBuilder.DropTable(
                name: "Administrations");

            migrationBuilder.DropTable(
                name: "AuthorizedUsers");
        }
    }
}
