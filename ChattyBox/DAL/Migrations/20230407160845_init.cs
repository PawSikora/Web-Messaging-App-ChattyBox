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
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
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
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_UserChats_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
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
                values: new object[] { 1, new DateTime(2023, 4, 7, 18, 8, 45, 165, DateTimeKind.Local).AddTicks(2786), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 4, 7, 18, 8, 45, 165, DateTimeKind.Local).AddTicks(2685), "marcinq@gmail.com", null, new byte[] { 31, 57, 181, 51, 16, 12, 140, 228, 34, 76, 111, 213, 183, 51, 76, 196, 225, 199, 115, 11, 186, 129, 22, 227, 9, 206, 68, 116, 98, 164, 111, 109, 171, 157, 245, 103, 120, 100, 32, 0, 135, 200, 117, 206, 164, 80, 176, 167, 107, 152, 237, 220, 208, 231, 42, 208, 124, 147, 219, 232, 147, 61, 126, 72 }, new byte[] { 86, 245, 165, 229, 221, 131, 49, 166, 109, 115, 113, 176, 154, 143, 7, 138, 127, 41, 122, 187, 154, 133, 58, 7, 36, 142, 82, 231, 236, 169, 28, 249, 125, 134, 51, 7, 181, 199, 50, 145, 181, 242, 162, 42, 23, 171, 144, 6, 32, 250, 78, 148, 72, 112, 49, 177, 242, 193, 68, 140, 215, 93, 132, 249, 12, 56, 109, 107, 55, 38, 24, 163, 16, 183, 48, 232, 45, 99, 43, 242, 214, 63, 84, 184, 85, 47, 18, 193, 199, 5, 180, 51, 243, 162, 50, 34, 78, 71, 164, 151, 89, 181, 20, 240, 20, 111, 141, 100, 13, 200, 163, 114, 72, 58, 139, 159, 155, 131, 82, 99, 72, 124, 35, 154, 64, 60, 79, 46 }, "MarIwin" },
                    { 2, new DateTime(2023, 4, 7, 18, 8, 45, 165, DateTimeKind.Local).AddTicks(2725), "tymonq@gmail.com", null, new byte[] { 118, 172, 116, 172, 183, 24, 239, 102, 45, 79, 46, 117, 13, 62, 24, 245, 78, 226, 42, 201, 213, 215, 183, 0, 168, 63, 179, 140, 181, 54, 56, 99, 100, 59, 95, 148, 175, 248, 189, 181, 200, 53, 184, 135, 137, 145, 182, 216, 24, 254, 76, 159, 0, 28, 146, 56, 122, 59, 240, 118, 170, 80, 54, 39 }, new byte[] { 179, 175, 68, 186, 88, 49, 149, 104, 209, 103, 173, 217, 78, 161, 16, 61, 50, 2, 21, 47, 87, 235, 46, 131, 146, 91, 31, 221, 134, 69, 85, 189, 76, 166, 94, 209, 164, 60, 6, 40, 242, 53, 216, 169, 69, 115, 160, 37, 9, 175, 175, 59, 200, 222, 47, 163, 244, 174, 120, 237, 123, 186, 119, 237, 228, 107, 160, 17, 146, 170, 123, 64, 108, 168, 249, 245, 16, 72, 79, 40, 139, 82, 114, 128, 75, 94, 255, 197, 223, 4, 40, 21, 65, 8, 125, 203, 161, 111, 12, 245, 140, 5, 34, 30, 226, 100, 133, 247, 249, 35, 180, 77, 155, 176, 241, 183, 1, 12, 94, 95, 3, 61, 75, 51, 250, 234, 246, 120 }, "TymonSme" }
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
                columns: new[] { "ChatId", "UserId", "RoleId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 1, 2, 2 }
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
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

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
                name: "IX_UserChats_RoleId",
                table: "UserChats",
                column: "RoleId");

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
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
