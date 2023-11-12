using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_project.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityToDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "ImageResultStatus",
                table: "ImageRes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchImageDiagnosisId1",
                table: "ImageRes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageResultStatus",
                table: "ImageRes",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
    }
}
