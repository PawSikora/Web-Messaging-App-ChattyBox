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
                values: new object[] { 1, new DateTime(2023, 3, 24, 21, 11, 57, 971, DateTimeKind.Local).AddTicks(8046), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 24, 21, 11, 57, 971, DateTimeKind.Local).AddTicks(7965), "marcinq@gmail.com", null, new byte[] { 126, 135, 76, 203, 133, 71, 137, 145, 133, 213, 52, 159, 48, 15, 106, 102, 193, 199, 58, 84, 171, 14, 235, 239, 247, 13, 78, 90, 89, 1, 189, 178, 26, 18, 85, 212, 58, 176, 94, 56, 58, 224, 1, 34, 90, 32, 102, 182, 31, 165, 177, 238, 207, 173, 180, 90, 135, 24, 85, 235, 113, 72, 50, 128 }, new byte[] { 160, 204, 45, 29, 240, 97, 178, 131, 53, 108, 56, 239, 139, 12, 175, 55, 180, 155, 12, 213, 245, 189, 137, 36, 117, 31, 207, 71, 24, 230, 107, 133, 6, 230, 195, 176, 70, 128, 97, 34, 133, 248, 247, 5, 162, 93, 97, 175, 97, 194, 183, 240, 254, 28, 59, 130, 186, 192, 142, 232, 186, 144, 213, 223, 182, 153, 175, 146, 104, 32, 198, 124, 217, 62, 184, 61, 40, 157, 98, 182, 181, 59, 17, 24, 100, 162, 52, 170, 168, 168, 124, 38, 202, 176, 0, 191, 114, 35, 234, 238, 181, 252, 11, 50, 143, 182, 43, 34, 25, 116, 210, 73, 105, 60, 143, 215, 47, 186, 168, 115, 78, 222, 71, 58, 223, 69, 81, 228 }, "MarIwin" },
                    { 2, new DateTime(2023, 3, 24, 21, 11, 57, 971, DateTimeKind.Local).AddTicks(8003), "tymonq@gmail.com", null, new byte[] { 108, 223, 102, 135, 225, 250, 109, 183, 17, 126, 12, 88, 163, 69, 25, 53, 23, 96, 83, 125, 128, 247, 66, 37, 186, 234, 128, 1, 249, 217, 153, 0, 99, 89, 178, 225, 159, 226, 156, 102, 250, 255, 123, 34, 77, 75, 7, 253, 79, 98, 247, 199, 75, 131, 111, 32, 39, 148, 11, 71, 47, 19, 247, 7 }, new byte[] { 129, 158, 24, 119, 145, 215, 59, 173, 4, 161, 56, 29, 57, 221, 110, 240, 174, 61, 212, 224, 209, 74, 248, 248, 114, 159, 75, 70, 103, 226, 152, 167, 141, 213, 174, 80, 8, 17, 177, 84, 27, 89, 255, 29, 218, 131, 231, 111, 211, 195, 54, 238, 179, 194, 120, 11, 206, 119, 201, 215, 222, 42, 22, 188, 56, 92, 159, 99, 34, 252, 72, 241, 168, 129, 27, 31, 80, 140, 65, 253, 112, 29, 36, 103, 227, 10, 134, 131, 46, 231, 229, 14, 191, 120, 94, 182, 235, 17, 237, 77, 65, 13, 236, 143, 190, 50, 46, 133, 44, 175, 224, 98, 40, 115, 254, 69, 74, 85, 186, 235, 107, 233, 228, 227, 226, 178, 42, 163 }, "TymonSme" }
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
