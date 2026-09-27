# Biology catalog and spawn rules

The catalog contains **115 unique species, 254 alternative rules, 19 source files,
11 body types**, imported from Silarn/EDMC-BioScan at commit
`fc6b85c3de48c8fb79fa9e4f79ad780e48f56629`.

This change supplies the database and seed data. Species prediction in
`ExobiologyViewModel` is the next step; the existing signal display still shows
confirmed species and genus placeholders.

## Relationships

| Table | Purpose |
|---|---|
| `BioSpecies` | Species, journal identifier, value, sampling distance, variant determinant |
| `BioSpawnRules` | Multiple alternative rules per species; nullable numeric bounds and provenance |
| `BodyTypes` | One lookup row per planet type, with an integer primary key |
| `BioSpawnRuleBodyTypes` | `(SpawnRuleId, BodyTypeId, Mode)` foreign-key links; no body names in rules |
| `BioSpawnRuleSystemBodyTypes` | Body types that must occur somewhere in the system |
| `Atmospheres` / `BioSpawnRuleAtmospheres` | Atmospheric types and per-rule foreign-key links |
| `BioSpawnRuleAtmosphereComponents` | Minimum percentage of a gas, such as sulphur dioxide |
| `BioSpawnRuleVolcanisms` | Volcanism patterns with explicit `Contains` or `Exact` matching |
| `BioSpawnRuleStars` | Star type, optional luminosity, and `System`/`Parent` scope |
| `BioCatalogVersions` | Content hash and upstream revision for idempotent startup imports |

The existing `BodyTypes` and `BioSpawnRuleBodyTypes` tables are reused. Names such
as `Rocky body` are stored once in the lookup table so journal values can still
be matched to IDs. The seed JSON also has separate lookup tables and numeric
`bodyTypeIds`. These JSON IDs are local references, resolved to actual database
IDs by the seeder; existing installed IDs are never overwritten.

A species matches if **any complete rule** matches. Conditions inside a rule
are **AND**. Members of atmosphere, body-type, volcanism-pattern and star lists
are **OR** alternatives. An empty allowed-type list means no restriction;
`None` atmosphere or volcanism explicitly means absence, not an unknown value.
A null numeric bound means no constraint. Missing observed planet data must not
be turned into a fabricated zero during future prediction.

## Imported fields and units

| Source | Storage / interpretation |
|---|---|
| `min_temperature`, `max_temperature` | Kelvin; inclusive limits |
| `min_gravity`, `max_gravity` | Source gravity units; journal m/s² divided by **9.797759**; inclusive |
| `min_pressure`, `max_pressure` | Source atmosphere units; journal Pa divided by **101231.656250**; minimum inclusive, maximum exclusive |
| `max_orbital_period` | Seconds, exclusive maximum |
| `distance` | Minimum distance from arrival in light-seconds, inclusive |
| `atmosphere_component` | Percentage from 0 to 100, inclusive minimum |
| `volcanism` | Unrestricted, None, Any, or OR-list of exact/substring patterns |
| `star` | Any star in the system; preserve star class plus optional luminosity |
| `parent_star` | Main star or any parent star, following the upstream evaluator |
| `bodies` | At least one matching body type anywhere in the system |
| `nebula` | Preserve `all`: nebula-sector membership, <150 ly from a large nebula, or <100 ly from a planetary nebula |

The conversion factors above intentionally follow the source evaluator rather
than silently substituting standard Earth gravity or standard atmosphere.
Star matching will also need the source's supergiant aliases, prefix handling
for D/C/W stars, and luminosity suffixes when prediction is implemented.

Galactic location fields **`regions`, `region`, `guardian`, and `tuber`** are
omitted as requested. The last two describe coordinate-based zones. No galaxy
region or zone tables are introduced. Nebula proximity is retained separately.
Ignoring these location restrictions broadens the eventual candidate list.

All 254 source alternatives are retained, even if removing region restrictions
makes two alternatives identical. `SourceFile` and `SourceIndex` identify each
original rule. The source's duplicate, empty `Stratum Aranaemus` alias is folded
into the populated species with the same journal ID, `Stratum Araneamus`.
The three list-form A-star/luminosity pairs in `anemone.py` are normalized like
the tuple-form pairs, preserving both their values.

## Startup and upgrades

`BioDataSeeder` reads the embedded JSON, applies migrations, and imports the
catalog transactionally. A content hash makes subsequent startup imports a
no-op. A changed catalog replaces rules only for matching catalog species;
unrelated species survive. Species, genus, body-type and atmosphere IDs are
reused, including old spacing/case variants. Existing species display names,
sampling distances and variant determinants are preserved. Catalog values and
journal identifiers are updated.

The migration moves existing species-level atmosphere conditions onto their
existing rules before dropping the old relation table. Legacy raw atmosphere
and volcanism columns remain for custom/unimported reference records; imported
rules use the typed relations exclusively. Such legacy-only custom rules must
be normalized before a future predictor evaluates their raw conditions.

The existing 90 species' sampling distances and variant determinants are
preserved in `species-metadata.json` for fresh installations. The 25 additional
species have unknown sampling distances (`null`) and an `Unknown` determinant
because the requested source files do not supply those values. The UI displays
an unknown distance as a dash instead of a fabricated number.

Make a copy of the installed SQLite database before first launching this branch.
Do not delete the existing database or migration history. A downgrade requires
restoring that backup: the old one-rule schema cannot represent alternatives,
so this migration deliberately refuses a lossy `Down` operation.

## Regenerate the embedded catalog

```sh
git clone https://github.com/Silarn/EDMC-BioScan.git /tmp/EDMC-BioScan
git -C /tmp/EDMC-BioScan checkout fc6b85c3de48c8fb79fa9e4f79ad780e48f56629
python tools/import_bioscan.py /tmp/EDMC-BioScan
python tools/import_bioscan.py /tmp/EDMC-BioScan --check
```

The importer uses `ast.literal_eval`, never executes upstream Python, and rejects
unknown fields, unexpected revisions and invalid ranges. Review the field
mapping before changing the pinned revision. Source-file hashes and omitted
field counts are included in the JSON for auditing.

## Attribution

Rule data is adapted from [Silarn/EDMC-BioScan](https://github.com/Silarn/EDMC-BioScan),
licensed under the GNU General Public License version 2. The upstream license is
included at `ED.Assistant/Data/Seed/Catalog/EDMC-BioScan-LICENSE.txt` and copied
with the application. The conversion and omission of geographic fields were
made for ED Assistant on 2026-09-27. Source data and its derived catalog carry
that license; retain the attribution and license with redistributions.
