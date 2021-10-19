using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dovecord.Data.Migrations
{
    public partial class Azure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new Guid("6ea179da-31bb-4a07-adbd-3f5713a1f8dd"), "General" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("1757fdac-99de-4a2b-9032-4604d948aeab"), "Random" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("249a1060-2257-4dba-8e8b-12aedf1f3349"), new Guid("6ea179da-31bb-4a07-adbd-3f5713a1f8dd"), "First ever channel message", new DateTime(2021, 10, 19, 15, 9, 35, 553, DateTimeKind.Local).AddTicks(5850), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChannelMessages",
                keyColumn: "Id",
                keyValue: new Guid("249a1060-2257-4dba-8e8b-12aedf1f3349"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("1757fdac-99de-4a2b-9032-4604d948aeab"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("6ea179da-31bb-4a07-adbd-3f5713a1f8dd"));

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
    }
}
