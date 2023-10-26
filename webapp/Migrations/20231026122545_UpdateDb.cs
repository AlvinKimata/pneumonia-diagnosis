using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_project.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchImageDiagnosisId1",
                table: "ImageRes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ImageRes_BatchImageDiagnosisId1",
                table: "ImageRes",
                column: "BatchImageDiagnosisId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRes_BatchImageDiagnosis_BatchImageDiagnosisId1",
                table: "ImageRes",
                column: "BatchImageDiagnosisId1",
                principalTable: "BatchImageDiagnosis",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageRes_BatchImageDiagnosis_BatchImageDiagnosisId1",
                table: "ImageRes");

            migrationBuilder.DropIndex(
                name: "IX_ImageRes_BatchImageDiagnosisId1",
                table: "ImageRes");

            migrationBuilder.DropColumn(
                name: "BatchImageDiagnosisId1",
                table: "ImageRes");
        }
    }
}
