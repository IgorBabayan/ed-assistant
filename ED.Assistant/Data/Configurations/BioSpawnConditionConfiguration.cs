namespace ED.Assistant.Data.Configurations;

sealed class BioSpawnConditionConfiguration : IEntityTypeConfiguration<BioSpawnCondition>
{
	public void Configure(EntityTypeBuilder<BioSpawnCondition> builder)
	{
		builder.ToTable("bio_spawn_condition");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.SpeciesId)
			.HasColumnName("species_id");

		builder.Property(x => x.VariantId)
			.HasColumnName("variant_id");

		builder.Property(x => x.Atmosphere)
			.HasColumnName("atmosphere")
			.HasMaxLength(150);

		builder.Property(x => x.PlanetClass)
			.HasColumnName("planet_class")
			.HasMaxLength(150);

		builder.Property(x => x.VolcanicActivity)
			.HasColumnName("volcanic_activity")
			.HasMaxLength(150);

		builder.Property(x => x.MinTemperatureK)
			.HasColumnName("min_temperature_k");

		builder.Property(x => x.MaxTemperatureK)
			.HasColumnName("max_temperature_k");

		builder.Property(x => x.MinGravityG)
			.HasColumnName("min_gravity_g");

		builder.Property(x => x.MaxGravityG)
			.HasColumnName("max_gravity_g");

		builder.Property(x => x.MinPressureAtm)
			.HasColumnName("min_pressure_atm");

		builder.Property(x => x.MaxPressureAtm)
			.HasColumnName("max_pressure_atm");

		builder.Property(x => x.MinDistanceFromStarLs)
			.HasColumnName("min_distance_from_star_ls");

		builder.Property(x => x.MaxDistanceFromStarLs)
			.HasColumnName("max_distance_from_star_ls");

		builder.Property(x => x.Notes)
			.HasColumnName("notes");

		builder.HasOne(x => x.Species)
			.WithMany(x => x.SpawnConditions)
			.HasForeignKey(x => x.SpeciesId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(x => x.Variant)
			.WithMany(x => x.SpawnConditions)
			.HasForeignKey(x => x.VariantId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(x => x.SpeciesId);
		builder.HasIndex(x => x.VariantId);

		builder.ToTable(x =>
		{
			x.HasCheckConstraint(
				"CK_bio_spawn_condition_species_or_variant",
				"species_id IS NOT NULL OR variant_id IS NOT NULL");
		});
	}
}