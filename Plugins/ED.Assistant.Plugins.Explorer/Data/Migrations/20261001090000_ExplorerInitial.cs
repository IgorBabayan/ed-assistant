using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Plugins.Explorer.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExplorerInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExplorerBody",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SystemAddress = table.Column<long>(type: "INTEGER", nullable: false),
                    BodyId = table.Column<int>(type: "INTEGER", nullable: false),
                    StarSystem = table.Column<string>(type: "TEXT", nullable: false),
                    BodyName = table.Column<string>(type: "TEXT", nullable: false),
                    PlanetClass = table.Column<string>(type: "TEXT", nullable: false),
                    IsTerraformable = table.Column<bool>(type: "INTEGER", nullable: false),
                    MassEm = table.Column<double>(type: "REAL", nullable: true),
                    WasDiscovered = table.Column<bool>(type: "INTEGER", nullable: false),
                    WasMapped = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsMapped = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsEfficientMapping = table.Column<bool>(type: "INTEGER", nullable: false),
                    DateScanned = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DateMapped = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateSold = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsLost = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Value = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorerBody", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExplorerJournalCursor",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    Position = table.Column<long>(type: "INTEGER", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExplorerJournalCursor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorerBody_IsActive_DateScanned",
                table: "ExplorerBody",
                columns: new[] { "IsActive", "DateScanned" });

            migrationBuilder.CreateIndex(
                name: "IX_ExplorerBody_SystemAddress_BodyId",
                table: "ExplorerBody",
                columns: new[] { "SystemAddress", "BodyId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ExplorerBody");
            migrationBuilder.DropTable(name: "ExplorerJournalCursor");
        }
    }
}
