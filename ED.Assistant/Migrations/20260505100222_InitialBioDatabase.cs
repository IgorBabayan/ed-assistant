using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class InitialBioDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "bio_genus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    display_name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_genus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bio_source",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_source", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "bio_species",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    genus_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    display_name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    base_value = table.Column<int>(type: "INTEGER", nullable: true),
                    min_scan_distance_m = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bio_species", x => x.Id);
                    table.ForeignKey(
                        name: "FK_bio_species_bio_genus_genus_id",
                        column: x => x.genus_id,
                        principalTable: "bio_genus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bio_variant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    species_id = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    display_name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    color_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    color_hex = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    image_url = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
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
                    species_id = table.Column<int>(type: "INTEGER", nullable: true),
                    variant_id = table.Column<int>(type: "INTEGER", nullable: true),
                    source_id = table.Column<int>(type: "INTEGER", nullable: false),
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
                    planet_class = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    volcanic_activity = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    min_temperature_k = table.Column<double>(type: "REAL", nullable: true),
                    max_temperature_k = table.Column<double>(type: "REAL", nullable: true),
                    min_gravity_g = table.Column<double>(type: "REAL", nullable: true),
                    max_gravity_g = table.Column<double>(type: "REAL", nullable: true),
                    min_pressure_atm = table.Column<double>(type: "REAL", nullable: true),
                    max_pressure_atm = table.Column<double>(type: "REAL", nullable: true),
                    min_distance_from_star_ls = table.Column<double>(type: "REAL", nullable: true),
                    max_distance_from_star_ls = table.Column<double>(type: "REAL", nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true)
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
                    star_class = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    material_name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    region_name = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    notes = table.Column<string>(type: "TEXT", nullable: true)
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
                name: "IX_bio_genus_name",
                table: "bio_genus",
                column: "name",
                unique: true);

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
                name: "IX_bio_species_genus_id",
                table: "bio_species",
                column: "genus_id");

            migrationBuilder.CreateIndex(
                name: "IX_bio_species_genus_id_name",
                table: "bio_species",
                columns: new[] { "genus_id", "name" },
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropTable(
                name: "bio_species");

            migrationBuilder.DropTable(
                name: "bio_genus");
        }
    }
}
