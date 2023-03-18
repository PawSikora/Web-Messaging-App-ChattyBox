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
                    Size = table.Column<double>(type: "float", nullable: false),
                    Extension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                values: new object[] { 1, new DateTime(2023, 3, 18, 17, 49, 11, 858, DateTimeKind.Local).AddTicks(6952), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 18, 17, 49, 11, 858, DateTimeKind.Local).AddTicks(6790), "marcinq@gmail.com", null, new byte[] { 50, 57, 60, 66, 143, 65, 97, 169, 251, 50, 92, 135, 81, 1, 34, 96, 161, 101, 228, 74, 34, 46, 120, 202, 147, 58, 237, 75, 15, 159, 87, 183, 26, 194, 107, 220, 161, 115, 132, 228, 146, 129, 155, 214, 170, 223, 135, 254, 222, 229, 118, 237, 23, 167, 221, 18, 43, 167, 72, 209, 37, 18, 217, 89 }, new byte[] { 242, 196, 159, 237, 82, 96, 148, 16, 113, 208, 114, 170, 206, 42, 93, 71, 138, 86, 23, 18, 30, 121, 130, 155, 237, 202, 82, 156, 141, 206, 193, 180, 1, 167, 69, 24, 229, 85, 181, 120, 247, 124, 231, 199, 119, 183, 56, 236, 210, 230, 130, 79, 216, 195, 61, 47, 173, 111, 43, 68, 11, 76, 22, 143, 131, 74, 129, 61, 146, 217, 186, 92, 102, 9, 229, 30, 176, 161, 157, 220, 28, 23, 112, 195, 185, 27, 4, 194, 28, 156, 180, 161, 4, 244, 109, 187, 134, 117, 224, 103, 216, 122, 132, 232, 12, 96, 33, 137, 224, 42, 215, 126, 111, 143, 215, 234, 174, 153, 189, 133, 21, 246, 121, 12, 5, 199, 162, 11 }, "MarIwin" },
                    { 2, new DateTime(2023, 3, 18, 17, 49, 11, 858, DateTimeKind.Local).AddTicks(6822), "tymonq@gmail.com", null, new byte[] { 124, 55, 186, 225, 251, 71, 3, 244, 24, 138, 221, 246, 104, 18, 212, 115, 204, 133, 71, 73, 102, 182, 170, 230, 47, 146, 157, 237, 8, 53, 180, 134, 179, 194, 223, 42, 88, 136, 58, 232, 91, 79, 125, 43, 93, 122, 214, 38, 116, 47, 148, 193, 19, 65, 102, 250, 108, 26, 128, 233, 41, 127, 0, 217 }, new byte[] { 9, 131, 105, 67, 107, 206, 238, 145, 133, 36, 98, 161, 195, 210, 205, 116, 77, 168, 252, 186, 132, 214, 222, 179, 58, 90, 31, 253, 82, 226, 63, 107, 248, 114, 175, 22, 86, 178, 176, 180, 230, 103, 249, 133, 41, 190, 124, 115, 118, 136, 177, 151, 193, 18, 164, 92, 237, 38, 182, 75, 88, 3, 9, 200, 70, 54, 59, 249, 172, 218, 95, 11, 228, 135, 9, 184, 70, 139, 46, 201, 96, 151, 201, 40, 72, 41, 1, 222, 86, 133, 133, 196, 213, 46, 40, 96, 58, 154, 97, 210, 112, 19, 105, 57, 71, 36, 54, 209, 239, 191, 74, 22, 98, 189, 229, 103, 18, 178, 167, 14, 126, 96, 129, 26, 60, 40, 8, 191 }, "TymonSme" }
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
