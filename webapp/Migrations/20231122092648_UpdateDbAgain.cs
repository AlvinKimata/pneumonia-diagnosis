using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace school_project.Migrations
{
    public partial class UpdateDbAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageRes_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "ImageRes");

            migrationBuilder.DropForeignKey(
                name: "FK_Photo_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "Photo");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRes_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "ImageRes",
                column: "BatchImageDiagnosisId",
                principalTable: "BatchImageDiagnosis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "Photo",
                column: "BatchImageDiagnosisId",
                principalTable: "BatchImageDiagnosis",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImageRes_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "ImageRes");

            migrationBuilder.DropForeignKey(
                name: "FK_Photo_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "Photo");

            migrationBuilder.AddForeignKey(
                name: "FK_ImageRes_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "ImageRes",
                column: "BatchImageDiagnosisId",
                principalTable: "BatchImageDiagnosis",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Photo_BatchImageDiagnosis_BatchImageDiagnosisId",
                table: "Photo",
                column: "BatchImageDiagnosisId",
                principalTable: "BatchImageDiagnosis",
                principalColumn: "Id");
        }
    }
}
