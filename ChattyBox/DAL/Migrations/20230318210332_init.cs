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
            migrationBuilder.CreateSequence(
                name: "MessageSequence");

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
                name: "FileMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [MessageSequence]"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ChatId = table.Column<int>(type: "int", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Size = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FileMessages_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FileMessages_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TextMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, defaultValueSql: "NEXT VALUE FOR [MessageSequence]"),
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
                name: "UserChat",
                columns: table => new
                {
                    ChatsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChat", x => new { x.ChatsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserChat_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChat_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "Id", "Created", "Name", "Updated" },
                values: new object[] { 1, new DateTime(2023, 3, 18, 22, 3, 32, 681, DateTimeKind.Local).AddTicks(6794), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 18, 22, 3, 32, 681, DateTimeKind.Local).AddTicks(6626), "marcinq@gmail.com", null, new byte[] { 62, 123, 191, 213, 14, 172, 19, 198, 189, 89, 61, 166, 104, 93, 33, 64, 86, 202, 52, 141, 134, 208, 177, 101, 116, 255, 53, 1, 197, 11, 169, 254, 50, 118, 238, 156, 54, 74, 42, 42, 49, 213, 151, 19, 22, 198, 199, 234, 230, 190, 74, 138, 133, 174, 214, 4, 44, 31, 61, 89, 88, 240, 245, 149 }, new byte[] { 48, 178, 47, 58, 180, 228, 69, 35, 203, 226, 227, 241, 26, 88, 48, 168, 160, 177, 88, 195, 89, 233, 202, 92, 244, 51, 158, 169, 108, 60, 222, 188, 50, 223, 245, 163, 135, 168, 146, 69, 71, 166, 132, 104, 25, 179, 242, 88, 241, 203, 51, 223, 88, 45, 42, 173, 151, 110, 72, 176, 92, 53, 128, 241, 210, 111, 96, 60, 130, 178, 138, 76, 51, 17, 225, 239, 176, 146, 94, 248, 108, 17, 204, 146, 81, 18, 3, 252, 66, 94, 190, 228, 192, 243, 240, 156, 47, 103, 200, 90, 242, 251, 11, 55, 163, 120, 188, 130, 104, 70, 34, 236, 177, 183, 140, 164, 93, 41, 238, 207, 0, 208, 82, 228, 185, 183, 73, 183 }, "MarIwin" },
                    { 2, new DateTime(2023, 3, 18, 22, 3, 32, 681, DateTimeKind.Local).AddTicks(6659), "tymonq@gmail.com", null, new byte[] { 62, 223, 139, 86, 225, 254, 137, 231, 203, 40, 155, 213, 147, 148, 0, 57, 53, 149, 36, 31, 54, 118, 165, 48, 139, 122, 202, 80, 195, 201, 119, 185, 64, 115, 107, 215, 224, 82, 13, 113, 205, 214, 78, 255, 28, 32, 162, 81, 77, 42, 84, 155, 194, 247, 212, 41, 130, 94, 165, 160, 152, 150, 7, 47 }, new byte[] { 14, 170, 88, 23, 76, 8, 177, 138, 136, 130, 18, 18, 111, 42, 11, 16, 159, 67, 219, 206, 236, 231, 56, 177, 178, 94, 137, 133, 132, 157, 58, 31, 24, 28, 160, 151, 9, 45, 111, 243, 98, 249, 230, 207, 20, 206, 121, 137, 58, 81, 10, 1, 108, 82, 70, 208, 218, 195, 89, 211, 196, 198, 132, 8, 112, 137, 224, 234, 153, 247, 146, 195, 221, 113, 255, 71, 233, 9, 53, 95, 85, 42, 40, 244, 116, 151, 124, 7, 20, 186, 29, 27, 16, 61, 203, 60, 171, 205, 39, 115, 171, 125, 197, 227, 153, 86, 8, 133, 45, 95, 131, 252, 199, 117, 200, 144, 1, 219, 67, 6, 16, 88, 147, 15, 252, 12, 182, 66 }, "TymonSme" }
                });

            migrationBuilder.InsertData(
                table: "TextMessages",
                columns: new[] { "Id", "ChatId", "Content", "SenderId", "TimeStamp" },
                values: new object[,]
                {
                    { 1, 1, "Hello1", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "Hello2", 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_Name",
                table: "Chats",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileMessages_ChatId",
                table: "FileMessages",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_FileMessages_SenderId",
                table: "FileMessages",
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
                name: "IX_UserChat_UsersId",
                table: "UserChat",
                column: "UsersId");

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
                name: "FileMessages");

            migrationBuilder.DropTable(
                name: "TextMessages");

            migrationBuilder.DropTable(
                name: "UserChat");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropSequence(
                name: "MessageSequence");
        }
    }
}
