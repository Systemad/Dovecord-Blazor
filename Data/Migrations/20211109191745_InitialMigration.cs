using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dovecord.Data.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    Online = table.Column<bool>(type: "INTEGER", nullable: false),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ChannelMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsEdit = table.Column<bool>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChannelId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChannelMessages_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalTable: "Channels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChannelMessages_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("062fe79b-61eb-4d21-8085-52a7cf954e13"), "Random" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8200a638-c9c0-43d0-909a-a89ddbd94c39"), "General" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Online", "UserId", "Username" },
                values: new object[] { new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), false, null, "danova" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("7edec147-cdfe-4356-80d0-08fa9faec9ea"), new Guid("8200a638-c9c0-43d0-909a-a89ddbd94c39"), "First ever channel message", new DateTime(2021, 11, 9, 20, 17, 44, 578, DateTimeKind.Local).AddTicks(4911), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessages_ChannelId",
                table: "ChannelMessages",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMessages_UserId",
                table: "ChannelMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserId",
                table: "Users",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelMessages");

            migrationBuilder.DropTable(
                name: "Channels");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
