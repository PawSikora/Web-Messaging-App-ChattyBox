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
                values: new object[] { 1, new DateTime(2023, 3, 26, 16, 22, 14, 705, DateTimeKind.Local).AddTicks(3891), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 26, 16, 22, 14, 705, DateTimeKind.Local).AddTicks(3806), "marcinq@gmail.com", null, new byte[] { 30, 209, 255, 46, 27, 90, 158, 188, 116, 9, 164, 135, 130, 183, 113, 99, 100, 127, 70, 207, 100, 69, 200, 211, 141, 127, 31, 127, 85, 25, 232, 169, 110, 148, 224, 188, 131, 5, 216, 116, 51, 177, 79, 72, 162, 91, 205, 217, 104, 155, 12, 216, 77, 182, 100, 150, 253, 159, 174, 84, 242, 103, 253, 83 }, new byte[] { 185, 218, 251, 115, 85, 150, 222, 215, 231, 97, 251, 50, 37, 161, 93, 217, 157, 177, 202, 139, 66, 23, 10, 243, 158, 50, 142, 74, 186, 15, 176, 224, 170, 132, 120, 173, 57, 73, 19, 159, 202, 37, 143, 58, 248, 229, 207, 236, 7, 21, 147, 168, 77, 126, 140, 82, 148, 238, 162, 208, 243, 172, 200, 123, 43, 252, 157, 175, 27, 137, 50, 69, 120, 73, 36, 12, 221, 7, 211, 167, 251, 61, 135, 50, 79, 251, 155, 156, 160, 189, 202, 160, 152, 197, 58, 51, 191, 27, 152, 129, 75, 74, 162, 226, 66, 103, 82, 105, 60, 89, 115, 32, 34, 236, 244, 116, 135, 131, 81, 166, 178, 171, 226, 0, 87, 29, 83, 208 }, "MarIwin" },
                    { 2, new DateTime(2023, 3, 26, 16, 22, 14, 705, DateTimeKind.Local).AddTicks(3840), "tymonq@gmail.com", null, new byte[] { 140, 152, 156, 194, 247, 206, 148, 56, 180, 98, 77, 93, 117, 237, 203, 208, 64, 189, 195, 170, 148, 112, 204, 72, 89, 54, 164, 246, 240, 237, 222, 1, 209, 58, 191, 185, 74, 156, 155, 113, 52, 12, 145, 236, 177, 113, 135, 236, 208, 155, 124, 9, 226, 27, 68, 9, 171, 221, 185, 184, 146, 231, 183, 204 }, new byte[] { 1, 217, 127, 32, 13, 74, 238, 54, 139, 96, 188, 208, 9, 161, 216, 70, 137, 115, 9, 2, 35, 31, 170, 118, 23, 163, 149, 93, 17, 88, 116, 81, 52, 69, 60, 14, 186, 52, 223, 35, 155, 33, 224, 225, 193, 216, 226, 85, 208, 210, 15, 207, 252, 225, 138, 150, 64, 153, 90, 88, 239, 217, 224, 196, 53, 166, 210, 122, 190, 235, 171, 230, 126, 188, 235, 76, 93, 150, 0, 205, 174, 193, 65, 254, 171, 80, 126, 128, 247, 37, 163, 194, 67, 255, 190, 189, 156, 155, 228, 242, 79, 18, 91, 59, 164, 235, 129, 4, 208, 4, 255, 35, 224, 80, 71, 124, 88, 181, 123, 252, 181, 52, 103, 84, 206, 171, 128, 143 }, "TymonSme" }
                });

            migrationBuilder.InsertData(
                table: "FileMessage",
                columns: new[] { "Id", "ChatId", "Name", "Path", "SenderId", "Size", "TimeStamp" },
                values: new object[,]
                {
                    { 2, 1, "File1.txt", "Path1", 1, 0.0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, "File2.txt", "Path1", 2, 0.0, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "TextMessages",
                columns: new[] { "Id", "ChatId", "Content", "SenderId", "TimeStamp" },
                values: new object[,]
                {
                    { 1, 1, "Hello1", 1, new DateTime(2019, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, "Hello2", 2, new DateTime(2021, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
