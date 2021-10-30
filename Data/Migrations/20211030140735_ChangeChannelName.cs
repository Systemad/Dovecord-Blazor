using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dovecord.Data.Migrations
{
    public partial class ChangeChannelName : Migration
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

            migrationBuilder.RenameColumn(
                name: "ChannelName",
                table: "Channels",
                newName: "Name");

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("58c1d9ab-a564-428e-aaba-af356a207193"), "Random" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("9e999e73-1a25-4e40-bb4a-3013dbdeeff2"), "General" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("c6bbe9c9-dd31-46c5-be80-fd8001f5cea7"), new Guid("9e999e73-1a25-4e40-bb4a-3013dbdeeff2"), "First ever channel message", new DateTime(2021, 10, 30, 16, 7, 34, 792, DateTimeKind.Local).AddTicks(3145), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChannelMessages",
                keyColumn: "Id",
                keyValue: new Guid("c6bbe9c9-dd31-46c5-be80-fd8001f5cea7"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("58c1d9ab-a564-428e-aaba-af356a207193"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("9e999e73-1a25-4e40-bb4a-3013dbdeeff2"));

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Channels",
                newName: "ChannelName");

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("5de94122-5ae6-451d-bcc2-61185547c859"), "Random" });

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("b6876e25-34c7-4749-aff7-19d116f4926d"), "General" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("e27eb46b-2885-45a7-98f2-15c048d9c64f"), new Guid("b6876e25-34c7-4749-aff7-19d116f4926d"), "First ever channel message", new DateTime(2021, 10, 13, 17, 29, 11, 754, DateTimeKind.Local).AddTicks(8876), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }
    }
}
