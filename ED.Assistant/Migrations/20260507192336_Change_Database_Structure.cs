using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class Change_Database_Structure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bio_species_bio_genus_genus_id",
                table: "bio_species");

            migrationBuilder.DropTable(
                name: "bio_reference");

            migrationBuilder.DropTable(
                name: "bio_spawn_condition");

            migrationBuilder.DropTable(
                name: "bio_variant_rule");

            migrationBuilder.DropTable(
                name: "bio_source");

            migrationBuilder.DropTable(
                name: "bio_variant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bio_species",
                table: "bio_species");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bio_genus",
                table: "bio_genus");

            migrationBuilder.DropColumn(
                name: "description",
                table: "bio_species");

            migrationBuilder.DropColumn(
                name: "description",
                table: "bio_genus");

            migrationBuilder.RenameTable(
                name: "bio_species",
                newName: "BioSpecies");

            migrationBuilder.RenameTable(
                name: "bio_genus",
                newName: "BioGenera");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "BioSpecies",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "min_scan_distance_m",
                table: "BioSpecies",
                newName: "MinScanDistanceM");

            migrationBuilder.RenameColumn(
                name: "genus_id",
                table: "BioSpecies",
                newName: "GenusId");

            migrationBuilder.RenameColumn(
                name: "display_name",
                table: "BioSpecies",
                newName: "DisplayName");

            migrationBuilder.RenameColumn(
                name: "base_value",
                table: "BioSpecies",
                newName: "BaseValue");

            migrationBuilder.RenameIndex(
                name: "IX_bio_species_genus_id_name",
                table: "BioSpecies",
                newName: "IX_BioSpecies_GenusId_Name");

            migrationBuilder.RenameIndex(
                name: "IX_bio_species_genus_id",
                table: "BioSpecies",
                newName: "IX_BioSpecies_GenusId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "BioGenera",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "display_name",
                table: "BioGenera",
                newName: "DisplayName");

            migrationBuilder.RenameIndex(
                name: "IX_bio_genus_name",
                table: "BioGenera",
                newName: "IX_BioGenera_Name");

            migrationBuilder.AlterColumn<int>(
                name: "MinScanDistanceM",
                table: "BioSpecies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BaseValue",
                table: "BioSpecies",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VariantDeterminant",
                table: "BioSpecies",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BioSpecies",
                table: "BioSpecies",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BioGenera",
                table: "BioGenera",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Atmospheres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atmospheres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BioSpawnRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
                    AtmosphereRaw = table.Column<string>(type: "TEXT", nullable: false),
                    BodyTypesRaw = table.Column<string>(type: "TEXT", nullable: false),
                    VolcanismRaw = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BioSpawnRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BioSpawnRules_BioSpecies_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "BioSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BodyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BodyTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesAtmosphereConditions",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
                    AtmosphereId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesAtmosphereConditions", x => new { x.SpeciesId, x.AtmosphereId, x.Mode });
                    table.ForeignKey(
                        name: "FK_SpeciesAtmosphereConditions_Atmospheres_AtmosphereId",
                        column: x => x.AtmosphereId,
                        principalTable: "Atmospheres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeciesAtmosphereConditions_BioSpecies_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "BioSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesBodyTypeConditions",
                columns: table => new
                {
                    SpeciesId = table.Column<int>(type: "INTEGER", nullable: false),
                    BodyTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Mode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesBodyTypeConditions", x => new { x.SpeciesId, x.BodyTypeId, x.Mode });
                    table.ForeignKey(
                        name: "FK_SpeciesBodyTypeConditions_BioSpecies_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "BioSpecies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SpeciesBodyTypeConditions_BodyTypes_BodyTypeId",
                        column: x => x.BodyTypeId,
                        principalTable: "BodyTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Atmospheres_Name",
                table: "Atmospheres",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BioSpawnRules_SpeciesId",
                table: "BioSpawnRules",
                column: "SpeciesId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BodyTypes_Name",
                table: "BodyTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesAtmosphereConditions_AtmosphereId",
                table: "SpeciesAtmosphereConditions",
                column: "AtmosphereId");

            migrationBuilder.CreateIndex(
                name: "IX_SpeciesBodyTypeConditions_BodyTypeId",
                table: "SpeciesBodyTypeConditions",
                column: "BodyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BioSpecies_BioGenera_GenusId",
                table: "BioSpecies",
                column: "GenusId",
                principalTable: "BioGenera",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BioSpecies_BioGenera_GenusId",
                table: "BioSpecies");

            migrationBuilder.DropTable(
                name: "BioSpawnRules");

            migrationBuilder.DropTable(
                name: "SpeciesAtmosphereConditions");

            migrationBuilder.DropTable(
                name: "SpeciesBodyTypeConditions");

            migrationBuilder.DropTable(
                name: "Atmospheres");

            migrationBuilder.DropTable(
                name: "BodyTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BioSpecies",
                table: "BioSpecies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BioGenera",
                table: "BioGenera");

            migrationBuilder.DropColumn(
                name: "VariantDeterminant",
                table: "BioSpecies");

            migrationBuilder.RenameTable(
                name: "BioSpecies",
                newName: "bio_species");

            migrationBuilder.RenameTable(
                name: "BioGenera",
                newName: "bio_genus");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "bio_species",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "MinScanDistanceM",
                table: "bio_species",
                newName: "min_scan_distance_m");

            migrationBuilder.RenameColumn(
                name: "GenusId",
                table: "bio_species",
                newName: "genus_id");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "bio_species",
                newName: "display_name");

            migrationBuilder.RenameColumn(
                name: "BaseValue",
                table: "bio_species",
                newName: "base_value");

            migrationBuilder.RenameIndex(
                name: "IX_BioSpecies_GenusId_Name",
                table: "bio_species",
                newName: "IX_bio_species_genus_id_name");

            migrationBuilder.RenameIndex(
                name: "IX_BioSpecies_GenusId",
                table: "bio_species",
                newName: "IX_bio_species_genus_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "bio_genus",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "DisplayName",
                table: "bio_genus",
                newName: "display_name");

            migrationBuilder.RenameIndex(
                name: "IX_BioGenera_Name",
                table: "bio_genus",
                newName: "IX_bio_genus_name");

            migrationBuilder.AlterColumn<int>(
                name: "min_scan_distance_m",
                table: "bio_species",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "base_value",
                table: "bio_species",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "bio_species",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "description",
                table: "bio_genus",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_bio_species",
                table: "bio_species",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bio_genus",
                table: "bio_genus",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "bio_source",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_source", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bio_variant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    species_id = table.Column<int>(type: "INTEGER", nullable: false),
                    color_hex = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    color_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    display_name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    image_url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_variant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bio_variant_bio_species_species_id",
                        column: x => x.species_id,
                        principalTable: "bio_species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bio_reference",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    genus_id = table.Column<int>(type: "INTEGER", nullable: true),
                    source_id = table.Column<int>(type: "INTEGER", nullable: false),
                    species_id = table.Column<int>(type: "INTEGER", nullable: true),
                    variant_id = table.Column<int>(type: "INTEGER", nullable: true),
                    source_url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_reference", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bio_reference_bio_genus_genus_id",
                        column: x => x.genus_id,
                        principalTable: "bio_genus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bio_reference_bio_source_source_id",
                        column: x => x.source_id,
                        principalTable: "bio_source",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bio_reference_bio_species_species_id",
                        column: x => x.species_id,
                        principalTable: "bio_species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bio_reference_bio_variant_variant_id",
                        column: x => x.variant_id,
                        principalTable: "bio_variant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bio_spawn_condition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    species_id = table.Column<int>(type: "INTEGER", nullable: true),
                    variant_id = table.Column<int>(type: "INTEGER", nullable: true),
                    atmosphere = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    max_distance_from_star_ls = table.Column<double>(type: "REAL", nullable: true),
                    max_gravity_g = table.Column<double>(type: "REAL", nullable: true),
                    max_pressure_atm = table.Column<double>(type: "REAL", nullable: true),
                    max_temperature_k = table.Column<double>(type: "REAL", nullable: true),
                    min_distance_from_star_ls = table.Column<double>(type: "REAL", nullable: true),
                    min_gravity_g = table.Column<double>(type: "REAL", nullable: true),
                    min_pressure_atm = table.Column<double>(type: "REAL", nullable: true),
                    min_temperature_k = table.Column<double>(type: "REAL", nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    planet_class = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    volcanic_activity = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_spawn_condition", x => x.Id);
                    table.CheckConstraint("CK_bio_spawn_condition_species_or_variant", "species_id IS NOT NULL OR variant_id IS NOT NULL");
                    table.ForeignKey(
                        name: "FK_bio_spawn_condition_bio_species_species_id",
                        column: x => x.species_id,
                        principalTable: "bio_species",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bio_spawn_condition_bio_variant_variant_id",
                        column: x => x.variant_id,
                        principalTable: "bio_variant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bio_variant_rule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    variant_id = table.Column<int>(type: "INTEGER", nullable: false),
                    material_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true),
                    region_name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    star_class = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_variant_rule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bio_variant_rule_bio_variant_variant_id",
                        column: x => x.variant_id,
                        principalTable: "bio_variant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bio_reference_genus_id",
                table: "bio_reference",
                column: "genus_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_reference_source_id",
                table: "bio_reference",
                column: "source_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_reference_species_id",
                table: "bio_reference",
                column: "species_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_reference_variant_id",
                table: "bio_reference",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_source_name",
                table: "bio_source",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bio_spawn_condition_species_id",
                table: "bio_spawn_condition",
                column: "species_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_spawn_condition_variant_id",
                table: "bio_spawn_condition",
                column: "variant_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_variant_species_id",
                table: "bio_variant",
                column: "species_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_variant_species_id_name",
                table: "bio_variant",
                columns: new[] { "species_id", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bio_variant_rule_material_name",
                table: "bio_variant_rule",
                column: "material_name");

            migrationBuilder.CreateIndex(
                name: "IX_bio_variant_rule_star_class",
                table: "bio_variant_rule",
                column: "star_class");

            migrationBuilder.CreateIndex(
                name: "IX_bio_variant_rule_variant_id",
                table: "bio_variant_rule",
                column: "variant_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bio_species_bio_genus_genus_id",
                table: "bio_species",
                column: "genus_id",
                principalTable: "bio_genus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
