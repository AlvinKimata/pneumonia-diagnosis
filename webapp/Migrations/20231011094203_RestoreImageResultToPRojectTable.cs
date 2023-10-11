﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_project.Migrations
{
    /// <inheritdoc />
    public partial class RestoreImageResultToPRojectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageResult",
                table: "SingleImageDiagnosis",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageResult",
                table: "SingleImageDiagnosis");
        }
    }
}
