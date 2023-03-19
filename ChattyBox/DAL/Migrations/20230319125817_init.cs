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
                values: new object[] { 1, new DateTime(2023, 3, 19, 13, 58, 17, 569, DateTimeKind.Local).AddTicks(8159), "Chat1", null });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Created", "Email", "LastLog", "PasswordHash", "PasswordSalt", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 3, 19, 13, 58, 17, 569, DateTimeKind.Local).AddTicks(8087), "marcinq@gmail.com", null, new byte[] { 2, 214, 250, 227, 52, 148, 206, 86, 183, 189, 252, 114, 188, 87, 162, 108, 209, 246, 97, 252, 65, 156, 40, 91, 82, 156, 43, 95, 144, 101, 28, 76, 166, 185, 206, 63, 46, 240, 185, 178, 59, 126, 134, 207, 140, 153, 177, 195, 43, 47, 111, 50, 64, 150, 104, 155, 87, 233, 245, 75, 1, 51, 100, 175 }, new byte[] { 202, 70, 138, 147, 90, 193, 239, 208, 93, 234, 58, 159, 215, 28, 104, 127, 10, 89, 202, 214, 131, 4, 68, 38, 142, 25, 148, 214, 178, 154, 21, 118, 136, 5, 83, 223, 73, 221, 185, 90, 189, 114, 164, 187, 217, 27, 250, 112, 196, 93, 72, 223, 68, 247, 8, 211, 48, 135, 165, 152, 94, 162, 254, 183, 221, 169, 0, 101, 133, 60, 36, 253, 151, 69, 214, 211, 6, 235, 75, 213, 238, 72, 109, 43, 51, 37, 42, 252, 228, 253, 135, 66, 252, 201, 83, 227, 61, 247, 231, 62, 251, 226, 188, 30, 68, 8, 137, 123, 193, 158, 120, 211, 166, 121, 23, 46, 207, 45, 147, 215, 214, 20, 133, 57, 216, 173, 153, 39 }, "MarIwin" },
                    { 2, new DateTime(2023, 3, 19, 13, 58, 17, 569, DateTimeKind.Local).AddTicks(8119), "tymonq@gmail.com", null, new byte[] { 17, 25, 234, 81, 186, 92, 130, 217, 104, 68, 79, 71, 233, 209, 175, 225, 152, 103, 15, 21, 54, 48, 22, 169, 64, 73, 150, 81, 200, 206, 160, 105, 17, 103, 108, 44, 167, 30, 235, 226, 17, 186, 93, 139, 48, 160, 207, 52, 195, 8, 51, 162, 105, 172, 120, 52, 245, 4, 96, 185, 187, 3, 0, 73 }, new byte[] { 2, 63, 134, 183, 45, 113, 95, 47, 93, 122, 198, 206, 252, 165, 176, 25, 16, 229, 37, 65, 226, 118, 147, 157, 10, 115, 101, 29, 76, 179, 182, 236, 227, 252, 139, 46, 211, 219, 224, 248, 120, 165, 176, 151, 8, 208, 205, 165, 49, 104, 108, 12, 123, 248, 210, 206, 238, 75, 86, 20, 51, 100, 121, 101, 63, 216, 119, 201, 1, 8, 253, 117, 228, 42, 66, 49, 18, 223, 96, 201, 76, 60, 19, 192, 191, 96, 22, 109, 132, 15, 87, 206, 67, 126, 31, 210, 247, 245, 57, 170, 56, 235, 105, 24, 26, 131, 73, 9, 241, 100, 28, 237, 172, 178, 89, 10, 138, 75, 217, 231, 146, 182, 85, 209, 73, 232, 29, 61 }, "TymonSme" }
                });

            migrationBuilder.InsertData(
                table: "TextMessages",
                columns: new[] { "Id", "ChatId", "Content", "SenderId", "TimeStamp" },
                values: new object[,]
                {
                    { 1, 1, "Hello1", 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, "Hello2", 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
                name: "FileMessages");

            migrationBuilder.DropTable(
                name: "TextMessages");

            migrationBuilder.DropTable(
                name: "UserChats");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropSequence(
                name: "MessageSequence");
        }
    }
}
