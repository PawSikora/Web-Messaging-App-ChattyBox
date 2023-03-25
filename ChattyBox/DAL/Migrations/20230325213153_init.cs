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
                values: new object[] { 1, new DateTime(2023, 3, 25, 22, 31, 53, 729, DateTimeKind.Local).AddTicks(296), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 25, 22, 31, 53, 729, DateTimeKind.Local).AddTicks(209), "marcinq@gmail.com", null, new byte[] { 252, 199, 6, 19, 93, 77, 249, 85, 81, 43, 139, 180, 221, 161, 199, 194, 32, 35, 99, 198, 97, 32, 244, 152, 185, 154, 209, 146, 132, 10, 246, 242, 141, 65, 91, 234, 152, 129, 116, 77, 235, 50, 52, 65, 84, 120, 88, 245, 180, 126, 70, 199, 252, 122, 129, 4, 201, 10, 99, 29, 2, 120, 64, 14 }, new byte[] { 28, 233, 226, 32, 157, 155, 196, 201, 57, 161, 195, 163, 22, 5, 62, 87, 205, 90, 7, 162, 27, 192, 137, 40, 110, 38, 205, 212, 189, 9, 186, 195, 34, 57, 250, 95, 80, 183, 234, 159, 155, 243, 11, 254, 82, 88, 35, 128, 31, 70, 242, 198, 124, 112, 11, 92, 118, 122, 195, 230, 132, 181, 220, 78, 154, 195, 89, 159, 223, 38, 34, 243, 148, 107, 160, 217, 88, 131, 210, 71, 50, 161, 78, 1, 33, 27, 118, 173, 148, 88, 132, 104, 150, 52, 123, 156, 12, 201, 161, 174, 250, 248, 90, 123, 236, 184, 129, 255, 120, 86, 163, 226, 208, 137, 139, 157, 151, 55, 189, 61, 190, 202, 97, 28, 144, 72, 103, 215 }, "MarIwin" },
                    { 2, new DateTime(2023, 3, 25, 22, 31, 53, 729, DateTimeKind.Local).AddTicks(243), "tymonq@gmail.com", null, new byte[] { 58, 24, 104, 222, 197, 45, 199, 84, 12, 255, 75, 226, 6, 1, 12, 3, 102, 97, 84, 70, 235, 203, 63, 94, 190, 129, 200, 74, 166, 43, 212, 41, 92, 99, 16, 164, 43, 30, 4, 129, 119, 61, 53, 93, 191, 148, 177, 175, 144, 30, 42, 10, 124, 81, 9, 195, 201, 197, 8, 226, 211, 146, 94, 82 }, new byte[] { 191, 235, 161, 38, 86, 157, 204, 229, 121, 211, 105, 174, 85, 132, 180, 64, 3, 11, 44, 134, 123, 186, 215, 68, 33, 169, 208, 29, 40, 102, 242, 119, 0, 57, 1, 163, 104, 114, 40, 119, 167, 134, 133, 157, 109, 7, 196, 216, 244, 229, 97, 92, 240, 137, 112, 1, 192, 183, 112, 144, 183, 234, 184, 103, 92, 32, 163, 155, 114, 47, 141, 178, 118, 206, 78, 251, 30, 212, 143, 135, 38, 153, 125, 251, 32, 247, 138, 81, 35, 39, 134, 34, 124, 106, 145, 103, 170, 145, 253, 131, 138, 67, 154, 153, 141, 133, 247, 227, 213, 194, 238, 244, 228, 2, 10, 168, 186, 79, 16, 83, 246, 231, 249, 32, 4, 93, 54, 130 }, "TymonSme" }
                });

            migrationBuilder.InsertData(
                table: "FileMessage",
                columns: new[] { "Id", "ChatId", "Name", "Path", "SenderId", "Size", "TimeStamp" },
                values: new object[,]
                {
                    { 2, 1, "File1.txt", "Path1", 1, 0.0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, "File2.txt", "Path1", 2, 0.0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
