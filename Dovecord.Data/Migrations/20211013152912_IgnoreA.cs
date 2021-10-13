using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dovecord.Data.Migrations
{
    public partial class IgnoreA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("b6876e25-34c7-4749-aff7-19d116f4926d"), "General" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("5de94122-5ae6-451d-bcc2-61185547c859"), "Random" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("e27eb46b-2885-45a7-98f2-15c048d9c64f"), new Guid("b6876e25-34c7-4749-aff7-19d116f4926d"), "First ever channel message", new DateTime(2021, 10, 13, 17, 29, 11, 754, DateTimeKind.Local).AddTicks(8876), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChannelMessages",
                keyColumn: "Id",
                keyValue: new Guid("e27eb46b-2885-45a7-98f2-15c048d9c64f"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("5de94122-5ae6-451d-bcc2-61185547c859"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("b6876e25-34c7-4749-aff7-19d116f4926d"));

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
    }
}
