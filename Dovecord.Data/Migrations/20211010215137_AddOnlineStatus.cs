using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dovecord.Data.Migrations
{
    public partial class AddOnlineStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChannelMessages",
                keyColumn: "Id",
                keyValue: new Guid("876b8de0-2380-4c48-a34f-e79ca8201245"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("90c08ba2-51ac-4e7a-b7b4-afa4295a092f"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("7ff2e577-ca2d-497a-adbb-970bd24c477f"));

            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("ff35c188-ce08-4e45-b9cc-9169d644d268"), "General" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("6d94c3f3-1df7-421c-9f49-de9de5c1e2f2"), "Random" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("50d99521-c47b-4c0e-9a85-9ecd836c87ef"), new Guid("ff35c188-ce08-4e45-b9cc-9169d644d268"), "First ever channel message", new DateTime(2021, 10, 10, 23, 51, 36, 225, DateTimeKind.Local).AddTicks(6067), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChannelMessages",
                keyColumn: "Id",
                keyValue: new Guid("50d99521-c47b-4c0e-9a85-9ecd836c87ef"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("6d94c3f3-1df7-421c-9f49-de9de5c1e2f2"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("ff35c188-ce08-4e45-b9cc-9169d644d268"));

            migrationBuilder.DropColumn(
                name: "Online",
                table: "Users");

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("7ff2e577-ca2d-497a-adbb-970bd24c477f"), "General" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("90c08ba2-51ac-4e7a-b7b4-afa4295a092f"), "Random" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("876b8de0-2380-4c48-a34f-e79ca8201245"), new Guid("7ff2e577-ca2d-497a-adbb-970bd24c477f"), "First ever channel message", new DateTime(2021, 9, 30, 16, 12, 33, 347, DateTimeKind.Local).AddTicks(1746), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }
    }
}
