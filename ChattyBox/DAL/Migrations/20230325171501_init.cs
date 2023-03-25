using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    LastLog = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FileMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "2, 2"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileMessage_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileMessage_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 2"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextMessages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChats",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChats", x => new { x.UserId, x.ChatId });
                    table.ForeignKey(
                        name: "FK_UserChats_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "Id", "Created", "Name", "Updated" },
                values: new object[] { 1, new DateTime(2023, 3, 25, 18, 15, 1, 639, DateTimeKind.Local).AddTicks(5532), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 25, 18, 15, 1, 639, DateTimeKind.Local).AddTicks(5442), "marcinq@gmail.com", null, new byte[] { 22, 138, 114, 204, 186, 100, 211, 214, 65, 101, 195, 227, 187, 69, 204, 218, 115, 197, 46, 70, 245, 169, 79, 96, 115, 178, 124, 190, 63, 57, 97, 100, 206, 122, 181, 159, 167, 204, 163, 29, 63, 110, 229, 113, 150, 107, 126, 76, 72, 214, 51, 221, 128, 210, 127, 123, 143, 12, 12, 78, 83, 147, 69, 168 }, new byte[] { 211, 78, 114, 243, 95, 245, 83, 29, 86, 87, 9, 206, 208, 95, 160, 127, 78, 41, 142, 254, 239, 141, 20, 34, 2, 145, 115, 42, 209, 29, 157, 126, 168, 199, 126, 196, 13, 18, 58, 180, 52, 182, 192, 177, 250, 30, 233, 181, 221, 84, 120, 226, 134, 97, 16, 130, 207, 132, 40, 252, 120, 254, 195, 109, 186, 197, 216, 244, 173, 90, 87, 189, 116, 33, 190, 247, 126, 111, 187, 188, 196, 39, 214, 120, 206, 160, 24, 19, 112, 136, 213, 140, 52, 17, 244, 247, 136, 246, 181, 46, 225, 127, 240, 31, 216, 134, 48, 235, 39, 71, 228, 16, 61, 39, 58, 254, 36, 137, 71, 95, 61, 67, 146, 237, 71, 226, 46, 39 }, "MarIwin" },
                    { 2, new DateTime(2023, 3, 25, 18, 15, 1, 639, DateTimeKind.Local).AddTicks(5483), "tymonq@gmail.com", null, new byte[] { 38, 109, 185, 39, 64, 215, 30, 79, 193, 148, 74, 44, 230, 38, 14, 75, 188, 125, 249, 170, 202, 90, 166, 199, 50, 136, 64, 120, 176, 43, 150, 218, 123, 146, 160, 124, 24, 170, 165, 64, 12, 95, 87, 169, 35, 137, 140, 249, 156, 122, 125, 158, 35, 186, 154, 37, 192, 1, 162, 94, 112, 20, 250, 111 }, new byte[] { 183, 234, 195, 165, 23, 99, 190, 110, 227, 109, 201, 108, 209, 104, 224, 15, 112, 182, 119, 167, 1, 92, 29, 122, 180, 203, 123, 197, 9, 73, 150, 232, 183, 91, 40, 12, 236, 220, 121, 187, 224, 159, 16, 77, 211, 253, 0, 114, 1, 201, 52, 97, 86, 99, 127, 182, 143, 93, 152, 62, 25, 65, 197, 33, 240, 174, 253, 199, 39, 165, 116, 26, 157, 109, 243, 107, 16, 10, 36, 195, 195, 233, 63, 141, 71, 178, 66, 247, 112, 139, 168, 27, 99, 218, 19, 37, 197, 22, 154, 139, 50, 223, 210, 225, 48, 81, 163, 104, 170, 33, 192, 113, 157, 182, 109, 52, 35, 151, 2, 24, 228, 58, 182, 143, 217, 49, 187, 97 }, "TymonSme" }
                });

            migrationBuilder.InsertData(
                table: "FileMessage",
                columns: new[] { "Id", "ChatId", "Name", "Path", "SenderId", "Size", "TimeStamp" },
                values: new object[,]
                {
                    { 2, 1, "Hello1", "Path1", 1, 0.0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, "Hello2", "Path1", 2, 0.0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "TextMessages",
                columns: new[] { "Id", "ChatId", "Content", "SenderId", "TimeStamp" },
                values: new object[,]
                {
                    { 1, 1, "Hello1", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, "Hello2", 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "UserChats",
                columns: new[] { "ChatId", "UserId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Name",
                table: "Chats",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileMessage_ChatId",
                table: "FileMessage",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMessage_SenderId",
                table: "FileMessage",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_TextMessages_ChatId",
                table: "TextMessages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_TextMessages_SenderId",
                table: "TextMessages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChats_ChatId",
                table: "UserChats",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileMessage");

            migrationBuilder.DropTable(
                name: "TextMessages");

            migrationBuilder.DropTable(
                name: "UserChats");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
