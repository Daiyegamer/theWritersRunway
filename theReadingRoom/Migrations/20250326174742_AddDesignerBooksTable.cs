using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdilBooks.Migrations
{
    /// <inheritdoc />
    public partial class AddDesignerBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignerBook_Books_BookId",
                table: "DesignerBook");

            migrationBuilder.DropForeignKey(
                name: "FK_DesignerBook_Designers_DesignerId",
                table: "DesignerBook");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignerBook",
                table: "DesignerBook");

            migrationBuilder.RenameTable(
                name: "DesignerBook",
                newName: "DesignerBooks");

            migrationBuilder.RenameIndex(
                name: "IX_DesignerBook_BookId",
                table: "DesignerBooks",
                newName: "IX_DesignerBooks_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignerBooks",
                table: "DesignerBooks",
                columns: new[] { "DesignerId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DesignerBooks_Books_BookId",
                table: "DesignerBooks",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DesignerBooks_Designers_DesignerId",
                table: "DesignerBooks",
                column: "DesignerId",
                principalTable: "Designers",
                principalColumn: "DesignerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DesignerBooks_Books_BookId",
                table: "DesignerBooks");

            migrationBuilder.DropForeignKey(
                name: "FK_DesignerBooks_Designers_DesignerId",
                table: "DesignerBooks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DesignerBooks",
                table: "DesignerBooks");

            migrationBuilder.RenameTable(
                name: "DesignerBooks",
                newName: "DesignerBook");

            migrationBuilder.RenameIndex(
                name: "IX_DesignerBooks_BookId",
                table: "DesignerBook",
                newName: "IX_DesignerBook_BookId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DesignerBook",
                table: "DesignerBook",
                columns: new[] { "DesignerId", "BookId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DesignerBook_Books_BookId",
                table: "DesignerBook",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DesignerBook_Designers_DesignerId",
                table: "DesignerBook",
                column: "DesignerId",
                principalTable: "Designers",
                principalColumn: "DesignerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
