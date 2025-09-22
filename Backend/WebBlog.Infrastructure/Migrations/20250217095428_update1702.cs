using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebBlog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update1702 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_PostReaction_PostId",
                table: "PostReaction",
                newName: "Index_PostReaction_PostId");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "CommentReaction",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentReaction_CommentId",
                table: "CommentReaction",
                newName: "Index_CommentReaction_CommentId");

            migrationBuilder.AddColumn<bool>(
                name: "HasChanged",
                table: "Comment",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Histories",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasChanged",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "Histories",
                table: "Comment");

            migrationBuilder.RenameIndex(
                name: "Index_PostReaction_PostId",
                table: "PostReaction",
                newName: "IX_PostReaction_PostId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "CommentReaction",
                newName: "User");

            migrationBuilder.RenameIndex(
                name: "Index_CommentReaction_CommentId",
                table: "CommentReaction",
                newName: "IX_CommentReaction_CommentId");
        }
    }
}
