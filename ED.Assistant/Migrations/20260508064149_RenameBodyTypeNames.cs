using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ED.Assistant.Migrations
{
    /// <inheritdoc />
    public partial class RenameBodyTypeNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'High metal content body'
			WHERE Name = 'HCS';
			""");

			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'Rocky body'
			WHERE Name = 'Rocky';
			""");

			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'Icy body'
			WHERE Name = 'Icy';
			""");

			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'Rocky Ice body'
			WHERE Name = 'Rocky Ice';
			""");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'HCS'
			WHERE Name = 'High metal content body';
			""");

			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'Rocky'
			WHERE Name = 'Rocky body';
			""");

			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'Icy'
			WHERE Name = 'Icy body';
			""");

			migrationBuilder.Sql("""
			UPDATE BodyTypes
			SET Name = 'Rocky Ice'
			WHERE Name = 'Rocky Ice body';
			""");
		}
    }
}
