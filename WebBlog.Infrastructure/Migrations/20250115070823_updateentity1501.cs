using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateentity1501 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_AppUsers_UserId",
                table: "PostReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReactions_Post_PostId",
                table: "PostReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions");

            migrationBuilder.RenameTable(
                name: "PostReactions",
                newName: "PostReaction");

            migrationBuilder.RenameIndex(
                name: "IX_PostReactions_UserId",
                table: "PostReaction",
                newName: "IX_PostReaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostReactions_PostId",
                table: "PostReaction",
                newName: "IX_PostReaction_PostId");

            migrationBuilder.AddColumn<bool>(
                name: "IsAnonymous",
                table: "Post",
                type: "bit",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReaction",
                table: "PostReaction",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "CommentReaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    User = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentReaction_Comment_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentReaction_CommentId",
                table: "CommentReaction",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReaction_AppUsers_UserId",
                table: "PostReaction",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReaction_Post_PostId",
                table: "PostReaction",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReaction_AppUsers_UserId",
                table: "PostReaction");

            migrationBuilder.DropForeignKey(
                name: "FK_PostReaction_Post_PostId",
                table: "PostReaction");

            migrationBuilder.DropTable(
                name: "CommentReaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostReaction",
                table: "PostReaction");

            migrationBuilder.DropColumn(
                name: "IsAnonymous",
                table: "Post");

            migrationBuilder.RenameTable(
                name: "PostReaction",
                newName: "PostReactions");

            migrationBuilder.RenameIndex(
                name: "IX_PostReaction_UserId",
                table: "PostReactions",
                newName: "IX_PostReactions_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_PostReaction_PostId",
                table: "PostReactions",
                newName: "IX_PostReactions_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostReactions",
                table: "PostReactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_AppUsers_UserId",
                table: "PostReactions",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReactions_Post_PostId",
                table: "PostReactions",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
