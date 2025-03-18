﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FashionVote.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Votes",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Votes");
        }
    }
}
