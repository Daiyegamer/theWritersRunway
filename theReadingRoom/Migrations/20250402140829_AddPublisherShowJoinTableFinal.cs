using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdilBooks.Migrations
{
    /// <inheritdoc />
    public partial class AddPublisherShowJoinTableFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublisherShow_Publishers_PublisherId",
                table: "PublisherShow");

            migrationBuilder.DropForeignKey(
                name: "FK_PublisherShow_Shows_ShowId",
                table: "PublisherShow");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherShow",
                table: "PublisherShow");

            migrationBuilder.RenameTable(
                name: "PublisherShow",
                newName: "PublisherShows");

            migrationBuilder.RenameIndex(
                name: "IX_PublisherShow_ShowId",
                table: "PublisherShows",
                newName: "IX_PublisherShows_ShowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherShows",
                table: "PublisherShows",
                columns: new[] { "PublisherId", "ShowId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PublisherShows_Publishers_PublisherId",
                table: "PublisherShows",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublisherShows_Shows_ShowId",
                table: "PublisherShows",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "ShowId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublisherShows_Publishers_PublisherId",
                table: "PublisherShows");

            migrationBuilder.DropForeignKey(
                name: "FK_PublisherShows_Shows_ShowId",
                table: "PublisherShows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PublisherShows",
                table: "PublisherShows");

            migrationBuilder.RenameTable(
                name: "PublisherShows",
                newName: "PublisherShow");

            migrationBuilder.RenameIndex(
                name: "IX_PublisherShows_ShowId",
                table: "PublisherShow",
                newName: "IX_PublisherShow_ShowId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PublisherShow",
                table: "PublisherShow",
                columns: new[] { "PublisherId", "ShowId" });

            migrationBuilder.AddForeignKey(
                name: "FK_PublisherShow_Publishers_PublisherId",
                table: "PublisherShow",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "PublisherId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PublisherShow_Shows_ShowId",
                table: "PublisherShow",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "ShowId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
