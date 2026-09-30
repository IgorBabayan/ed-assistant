using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class EvaluatorSampleLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BodyId",
                table: "Evaluator",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SystemAddress",
                table: "Evaluator",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluator_SystemAddress_BodyId_GenusId",
                table: "Evaluator",
                columns: new[] { "SystemAddress", "BodyId", "GenusId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Evaluator_SystemAddress_BodyId_GenusId",
                table: "Evaluator");

            migrationBuilder.DropColumn(
                name: "BodyId",
                table: "Evaluator");

            migrationBuilder.DropColumn(
                name: "SystemAddress",
                table: "Evaluator");
        }
    }
}
