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
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                values: new object[] { 1, new DateTime(2023, 5, 25, 19, 4, 41, 26, DateTimeKind.Local).AddTicks(1013), "Chat1", null });

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
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "RefreshToken", "TokenCreated", "TokenExpires", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 25, 19, 4, 41, 26, DateTimeKind.Local).AddTicks(681), "marcinq@gmail.com", null, new byte[] { 44, 188, 254, 200, 191, 65, 86, 220, 212, 29, 129, 115, 236, 58, 65, 110, 83, 18, 15, 51, 154, 102, 128, 191, 62, 106, 216, 2, 180, 58, 246, 75, 176, 158, 203, 193, 95, 63, 151, 38, 131, 59, 101, 156, 90, 178, 28, 194, 66, 4, 17, 214, 91, 129, 73, 235, 24, 51, 97, 3, 168, 117, 234, 221 }, new byte[] { 69, 76, 31, 68, 156, 11, 197, 132, 127, 235, 43, 234, 253, 82, 49, 100, 60, 42, 5, 164, 193, 99, 43, 43, 55, 206, 228, 121, 46, 27, 193, 89, 92, 42, 105, 140, 227, 251, 92, 112, 226, 78, 35, 177, 211, 108, 118, 209, 41, 43, 200, 204, 213, 220, 166, 186, 22, 183, 12, 167, 62, 118, 5, 167, 120, 190, 177, 112, 147, 83, 167, 225, 38, 229, 100, 4, 195, 176, 99, 192, 251, 32, 81, 138, 80, 182, 225, 45, 73, 3, 200, 21, 175, 25, 94, 201, 8, 9, 36, 12, 43, 24, 226, 82, 28, 175, 130, 173, 56, 224, 57, 235, 46, 52, 52, 93, 124, 125, 130, 141, 71, 0, 161, 229, 129, 36, 13, 35 }, "FL1RR9c+DTmRT20IyVPtFoIZYkzXxbPqN71oATGz4MVKBfBRCi+cEj4wg0IBt/0CCo6TyVOA4LzpGewzlZyPHA==", new DateTime(2023, 5, 25, 19, 4, 41, 26, DateTimeKind.Local).AddTicks(724), new DateTime(2023, 5, 26, 19, 4, 41, 26, DateTimeKind.Local).AddTicks(729), "MarIwin" },
                    { 2, new DateTime(2023, 5, 25, 19, 4, 41, 26, DateTimeKind.Local).AddTicks(752), "tymonq@gmail.com", null, new byte[] { 217, 120, 219, 130, 79, 199, 205, 222, 68, 156, 60, 56, 203, 199, 90, 157, 169, 168, 74, 65, 118, 232, 3, 42, 38, 236, 223, 52, 127, 171, 34, 84, 227, 10, 122, 32, 172, 88, 221, 250, 66, 136, 76, 7, 19, 248, 129, 17, 247, 159, 123, 226, 173, 23, 241, 244, 6, 42, 179, 196, 234, 126, 176, 250 }, new byte[] { 105, 149, 158, 90, 216, 239, 96, 5, 163, 88, 190, 102, 156, 148, 28, 189, 99, 69, 251, 195, 186, 125, 129, 86, 84, 93, 189, 189, 46, 136, 79, 96, 122, 135, 202, 141, 13, 88, 8, 184, 70, 76, 114, 183, 226, 27, 73, 235, 230, 218, 39, 205, 38, 134, 65, 246, 176, 48, 118, 18, 127, 82, 135, 157, 131, 92, 65, 49, 83, 203, 204, 35, 212, 51, 14, 138, 75, 212, 40, 153, 186, 61, 219, 159, 132, 112, 222, 174, 193, 246, 226, 83, 231, 217, 178, 213, 219, 240, 28, 2, 83, 81, 211, 231, 128, 169, 30, 143, 181, 24, 196, 177, 233, 48, 151, 31, 157, 67, 161, 16, 52, 120, 161, 245, 176, 179, 203, 65 }, "pnRvCXKg8Zz9xjTS/n2UBBO11MQB4OKpo27BVMYLKul4lL0h9GKrpcz4GiUDdT5iQS/x71KUjykQ/6O9C3j48w==", new DateTime(2023, 5, 25, 19, 4, 41, 26, DateTimeKind.Local).AddTicks(754), new DateTime(2023, 5, 26, 19, 4, 41, 26, DateTimeKind.Local).AddTicks(756), "TymonSme" }
                });

            migrationBuilder.InsertData(
                table: "FileMessage",
                columns: new[] { "Id", "ChatId", "Name", "Path", "SenderId", "Size", "TimeStamp" },
                values: new object[,]
                {
                    { 2, 1, "stockImage.jpg", "files\\Chat1\\stockImage.jpg", 1, 0.09969329833984375, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, "stockGif.gif", "files\\Chat1\\stockGif.gif", 2, 5.467991828918457, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
