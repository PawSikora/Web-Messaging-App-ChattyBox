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
                values: new object[] { 1, new DateTime(2023, 8, 14, 19, 13, 51, 286, DateTimeKind.Local).AddTicks(1631), "Chat1", null });

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
                    { 1, new DateTime(2023, 8, 14, 19, 13, 51, 286, DateTimeKind.Local).AddTicks(1226), "marcinq@gmail.com", null, new byte[] { 230, 244, 3, 124, 68, 198, 182, 80, 195, 216, 103, 47, 131, 228, 93, 48, 84, 55, 66, 123, 160, 55, 90, 21, 166, 86, 218, 149, 142, 153, 94, 87, 242, 143, 169, 210, 53, 115, 185, 233, 201, 112, 221, 38, 165, 220, 233, 179, 149, 73, 129, 137, 200, 154, 33, 168, 251, 96, 204, 240, 114, 157, 166, 102 }, new byte[] { 198, 164, 130, 12, 93, 104, 205, 244, 141, 65, 227, 137, 220, 250, 250, 148, 177, 141, 56, 88, 179, 47, 147, 250, 140, 49, 164, 45, 157, 51, 113, 36, 39, 76, 203, 183, 216, 223, 120, 166, 162, 102, 57, 196, 70, 230, 238, 182, 136, 173, 157, 202, 65, 109, 144, 204, 89, 186, 135, 76, 20, 68, 245, 124, 229, 65, 214, 79, 75, 227, 202, 121, 199, 229, 65, 184, 208, 145, 107, 135, 218, 41, 223, 213, 62, 181, 171, 159, 22, 36, 97, 143, 114, 135, 87, 85, 170, 193, 124, 163, 117, 27, 191, 152, 32, 104, 41, 167, 236, 105, 20, 50, 220, 208, 46, 234, 105, 20, 84, 216, 33, 192, 233, 232, 138, 20, 171, 104 }, "FiVJV+i5P5Wg+q8mScGggVMZUAeT7bYPM6X22vLhvWO7+MjC7lNth15Vj4eUcvj9FoUaayvxZ4G0BlEVNsbUUg==", new DateTime(2023, 8, 14, 19, 13, 51, 286, DateTimeKind.Local).AddTicks(1284), new DateTime(2023, 8, 15, 19, 13, 51, 286, DateTimeKind.Local).AddTicks(1291), "MarIwin" },
                    { 2, new DateTime(2023, 8, 14, 19, 13, 51, 286, DateTimeKind.Local).AddTicks(1340), "tymonq@gmail.com", null, new byte[] { 205, 82, 40, 199, 29, 153, 227, 103, 162, 64, 97, 64, 186, 95, 145, 248, 94, 28, 95, 21, 94, 22, 58, 186, 221, 22, 24, 48, 222, 156, 247, 180, 103, 192, 222, 54, 8, 236, 134, 213, 67, 240, 197, 114, 84, 161, 154, 218, 109, 68, 161, 126, 100, 27, 62, 33, 187, 197, 207, 200, 176, 94, 59, 199 }, new byte[] { 104, 147, 140, 53, 157, 197, 179, 36, 215, 46, 106, 144, 89, 30, 194, 145, 251, 161, 71, 239, 209, 235, 82, 60, 164, 7, 42, 193, 143, 163, 73, 114, 81, 121, 164, 246, 188, 72, 243, 27, 81, 113, 128, 149, 31, 135, 165, 255, 220, 91, 191, 5, 16, 52, 180, 65, 203, 154, 12, 26, 96, 108, 68, 243, 135, 132, 139, 113, 86, 95, 114, 166, 205, 246, 226, 174, 63, 65, 101, 223, 166, 68, 206, 162, 80, 139, 0, 78, 93, 114, 155, 70, 51, 26, 191, 110, 215, 4, 125, 159, 53, 16, 110, 109, 205, 63, 196, 32, 108, 114, 1, 111, 92, 16, 131, 40, 253, 120, 158, 97, 66, 120, 176, 139, 225, 15, 156, 164 }, "MtWiD9qFmAf2REQd6VCpodfsIBn1WLubjH4/DK4du8B0l3ErppjuVuxrCCZ/ayHwVyakA05Jv05bJmqkgskFnw==", new DateTime(2023, 8, 14, 19, 13, 51, 286, DateTimeKind.Local).AddTicks(1345), new DateTime(2023, 8, 15, 19, 13, 51, 286, DateTimeKind.Local).AddTicks(1347), "TymonSme" }
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
