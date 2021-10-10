using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dovecord.Data.Migrations
{
    public partial class AddCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("14620ada-964b-49ec-a0ca-f84f02b8c4ec"), "General" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("d3c496e5-ec35-4703-b154-649528fe29b8"), "Random" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("461f41b0-34ab-4453-8b62-9ccf6cd11ee7"), new Guid("14620ada-964b-49ec-a0ca-f84f02b8c4ec"), "First ever channel message", new DateTime(2021, 10, 11, 0, 12, 44, 177, DateTimeKind.Local).AddTicks(5278), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChannelMessages",
                keyColumn: "Id",
                keyValue: new Guid("461f41b0-34ab-4453-8b62-9ccf6cd11ee7"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("d3c496e5-ec35-4703-b154-649528fe29b8"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("14620ada-964b-49ec-a0ca-f84f02b8c4ec"));

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
    }
}
