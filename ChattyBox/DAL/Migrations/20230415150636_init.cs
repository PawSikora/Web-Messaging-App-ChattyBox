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
                values: new object[] { 1, new DateTime(2023, 4, 15, 17, 6, 35, 977, DateTimeKind.Local).AddTicks(2031), "Chat1", null });

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
                    { 1, new DateTime(2023, 4, 15, 17, 6, 35, 977, DateTimeKind.Local).AddTicks(1877), "marcinq@gmail.com", null, new byte[] { 161, 38, 163, 193, 95, 29, 23, 231, 170, 230, 102, 65, 150, 108, 203, 85, 180, 45, 30, 26, 0, 190, 233, 1, 202, 5, 253, 61, 122, 44, 23, 30, 208, 237, 198, 134, 211, 247, 123, 99, 214, 149, 53, 78, 177, 149, 209, 149, 55, 248, 145, 228, 58, 63, 241, 153, 71, 231, 159, 128, 38, 203, 3, 107 }, new byte[] { 90, 31, 130, 201, 175, 147, 108, 45, 222, 114, 122, 250, 2, 234, 209, 136, 23, 181, 247, 120, 191, 47, 107, 232, 57, 69, 193, 214, 65, 155, 129, 163, 196, 252, 175, 73, 117, 34, 2, 173, 180, 205, 80, 83, 243, 96, 186, 111, 90, 11, 120, 24, 252, 30, 228, 200, 183, 41, 124, 97, 200, 172, 70, 101, 99, 167, 53, 176, 163, 181, 59, 173, 112, 232, 136, 121, 183, 52, 5, 178, 169, 59, 152, 216, 151, 134, 63, 8, 76, 47, 54, 84, 10, 79, 153, 24, 82, 157, 1, 192, 243, 159, 230, 174, 229, 154, 60, 176, 164, 24, 170, 235, 25, 160, 176, 191, 27, 27, 12, 161, 161, 0, 80, 232, 67, 34, 174, 45 }, "MarIwin" },
                    { 2, new DateTime(2023, 4, 15, 17, 6, 35, 977, DateTimeKind.Local).AddTicks(1909), "tymonq@gmail.com", null, new byte[] { 124, 71, 27, 48, 8, 15, 70, 136, 23, 139, 223, 156, 113, 94, 174, 28, 144, 194, 2, 247, 68, 7, 116, 136, 15, 135, 35, 40, 57, 118, 164, 237, 97, 208, 214, 138, 21, 91, 17, 82, 54, 22, 215, 55, 50, 104, 104, 238, 42, 137, 212, 61, 61, 28, 174, 81, 88, 205, 7, 221, 246, 43, 250, 6 }, new byte[] { 163, 138, 69, 154, 123, 109, 156, 109, 45, 11, 245, 127, 133, 117, 124, 176, 77, 153, 73, 169, 249, 71, 157, 161, 247, 143, 230, 213, 21, 38, 241, 215, 199, 116, 196, 238, 219, 124, 52, 140, 101, 99, 202, 36, 134, 169, 162, 160, 233, 118, 126, 142, 105, 92, 145, 247, 210, 211, 209, 80, 102, 48, 137, 92, 43, 166, 255, 160, 79, 40, 51, 224, 123, 110, 60, 199, 12, 101, 8, 57, 109, 28, 39, 193, 125, 117, 130, 31, 8, 74, 202, 155, 7, 207, 94, 118, 7, 174, 109, 17, 220, 161, 248, 45, 228, 248, 127, 20, 17, 179, 89, 208, 95, 222, 250, 121, 82, 167, 152, 47, 92, 212, 116, 114, 207, 64, 41, 99 }, "TymonSme" }
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
