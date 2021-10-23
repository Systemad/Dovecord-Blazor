using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Dovecord.Data.Migrations
{
    public partial class SeedSecondChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ChannelMessages",
                keyColumn: "Id",
                keyValue: new Guid("46483836-447a-4bfa-9929-3442047e8e0e"));

            migrationBuilder.DeleteData(
                table: "Channels",
                keyColumn: "Id",
                keyValue: new Guid("b5606a5a-a2e9-4ae8-a568-eb8c5aaed245"));

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "Channels",
                columns: new[] { "Id", "ChannelName" },
                values: new object[] { new Guid("b5606a5a-a2e9-4ae8-a568-eb8c5aaed245"), "General" });

            migrationBuilder.InsertData(
                table: "ChannelMessages",
                columns: new[] { "Id", "ChannelId", "Content", "CreatedAt", "IsEdit", "UserId", "Username" },
                values: new object[] { new Guid("46483836-447a-4bfa-9929-3442047e8e0e"), new Guid("b5606a5a-a2e9-4ae8-a568-eb8c5aaed245"), "First ever channel message", new DateTime(2021, 9, 22, 23, 55, 20, 969, DateTimeKind.Local).AddTicks(4226), false, new Guid("ca0f4479-5992-4a00-a3d5-d73ae1daff6f"), "danova" });
        }
    }
}
