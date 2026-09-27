#!/usr/bin/env python3
"""Convert a pinned EDMC-BioScan checkout to the embedded catalog; never execute it."""
import argparse
import ast
from collections import Counter
import hashlib
import json
from pathlib import Path
import subprocess

ROOT = Path(__file__).resolve().parents[1]
CATALOG = ROOT / 'ED.Assistant/Data/Seed/Catalog'
PINNED_COMMIT = 'fc6b85c3de48c8fb79fa9e4f79ad780e48f56629'
# These keys all describe galactic regions/zones, intentionally omitted.
SKIPPED = {'region', 'regions', 'guardian', 'tuber'}
NUMBERS = {
    'min_temperature': 'minTemperatureK', 'max_temperature': 'maxTemperatureK',
    'min_gravity': 'minGravityG', 'max_gravity': 'maxGravityG',
    'min_pressure': 'minPressureAtmospheres', 'max_pressure': 'maxPressureAtmospheres',
    'max_orbital_period': 'maxOrbitalPeriodSeconds', 'distance': 'minArrivalDistanceLs',
}
KNOWN = set(NUMBERS) | SKIPPED | {
    'atmosphere', 'atmosphere_component', 'body_type', 'bodies', 'volcanism',
    'star', 'parent_star', 'nebula',
}
GENUS_NAMES = {'anemone': 'Anemone', 'brain_tree': 'Brain Tree',
               'shard': 'Crystalline Shards', 'tubers': 'Sinuous Tubers'}


def read_catalog(path):
    tree = ast.parse(path.read_text(encoding='utf-8'))
    return next(ast.literal_eval(n.value) for n in tree.body
                if isinstance(n, ast.AnnAssign) and isinstance(n.target, ast.Name)
                and n.target.id == 'catalog')


def build(source):
    revision = subprocess.check_output(
        ['git', '-C', str(source), 'rev-parse', 'HEAD'], text=True).strip()
    if revision != PINNED_COMMIT:
        raise ValueError(f'Expected reviewed revision {PINNED_COMMIT}; got {revision}')
    metadata = json.loads((CATALOG / 'species-metadata.json').read_text())
    files = sorted((source / 'src/bio_scan/bio_data/rulesets').glob('*.py'))
    if len(files) != 19:
        raise ValueError('Expected all 19 reviewed ruleset files')
    entries, sources, skipped = {}, [], Counter()
    body_names, atmosphere_names = set(), set()
    aliases = []
    for path in files:
        sources.append({'file': path.name, 'sha256': hashlib.sha256(path.read_bytes()).hexdigest()})
        for genus_code, species in read_catalog(path).items():
            for code, item in species.items():
                # Upstream has a legacy Stratum spelling/genus alias with no rules.
                if code in entries and not item['rulesets']:
                    aliases.append({'journalName': code, 'name': item['name']})
                    continue
                if code in entries:
                    if entries[code]['rawRules']:
                        raise ValueError(f'Duplicate populated species {code}')
                    aliases.append({'journalName': code, 'name': entries[code]['name']})
                entries[code] = {
                    'journalName': code, 'genusJournalName': genus_code,
                    'genusName': GENUS_NAMES.get(path.stem, path.stem.capitalize()),
                    'name': item['name'], 'baseValue': item['value'],
                    **metadata.get(item['name'], {'minScanDistanceM': None, 'variantDeterminant': 'Unknown'}),
                    'sourceFile': path.name, 'rawRules': item['rulesets'],
                }
                for rule in item['rulesets']:
                    if unknown := set(rule) - KNOWN:
                        raise ValueError(f'Unmapped rule fields: {unknown}')
                    skipped.update(set(rule) & SKIPPED)
                    body_names.update(rule.get('body_type', []))
                    body_names.update(rule.get('bodies', []))
                    atmosphere_names.update(rule.get('atmosphere', []))
                    atmosphere_names.update(rule.get('atmosphere_component', {}))
    bodies = {name: index for index, name in enumerate(sorted(body_names), 1)}
    atmospheres = {name: index for index, name in enumerate(sorted(atmosphere_names), 1)}
    for item in entries.values():
        item['rules'] = []
        for index, raw in enumerate(item.pop('rawRules'), 1):
            rule = {'sourceIndex': index}
            rule.update({dest: raw[key] for key, dest in NUMBERS.items() if key in raw})
            rule['bodyTypeIds'] = [bodies[name] for name in raw.get('body_type', [])]
            rule['systemBodyTypeIds'] = [bodies[name] for name in raw.get('bodies', [])]
            rule['atmosphereIds'] = [atmospheres[name] for name in raw.get('atmosphere', [])]
            rule['atmosphereComponents'] = [
                {'atmosphereId': atmospheres[name], 'minPercent': percent}
                for name, percent in raw.get('atmosphere_component', {}).items()]
            volcanism = raw.get('volcanism')
            rule['volcanismMode'] = ('Unrestricted' if volcanism is None else
                                     'Patterns' if isinstance(volcanism, list) else volcanism)
            if rule['volcanismMode'] not in {'Unrestricted', 'Patterns', 'None', 'Any'}:
                raise ValueError(f'Unmapped volcanism mode {volcanism}')
            rule['volcanismPatterns'] = [
                {'pattern': value.lstrip('='), 'match': 'Exact' if value.startswith('=') else 'Contains'}
                for value in volcanism] if isinstance(volcanism, list) else []
            rule['stars'] = []
            for key, scope in [('star', 'System'), ('parent_star', 'Parent')]:
                for value in raw.get(key, []):
                    star_type, luminosity = value if isinstance(value, (tuple, list)) else (value, None)
                    rule['stars'].append({'scope': scope, 'starType': star_type, 'luminosity': luminosity})
            rule['nebula'] = raw.get('nebula')
            for low, high in [('minTemperatureK', 'maxTemperatureK'), ('minGravityG', 'maxGravityG'),
                              ('minPressureAtmospheres', 'maxPressureAtmospheres')]:
                if low in rule and high in rule and rule[low] > rule[high]:
                    raise ValueError(f'Invalid range in {item["name"]}: {rule}')
            item['rules'].append(rule)
    return {
        'schemaVersion': 1,
        'source': 'https://github.com/Silarn/EDMC-BioScan', 'sourceCommit': revision,
        'omittedGeographicFields': sorted(SKIPPED), 'omittedFieldCounts': dict(sorted(skipped.items())),
        'sourceFiles': sources, 'aliases': aliases,
        'bodyTypes': [{'id': id_, 'name': name} for name, id_ in bodies.items()],
        'atmospheres': [{'id': id_, 'name': name} for name, id_ in atmospheres.items()],
        'species': sorted(entries.values(), key=lambda item: item['journalName']),
    }


def main():
    parser = argparse.ArgumentParser(description=__doc__)
    parser.add_argument('source', type=Path, help='EDMC-BioScan checkout at the pinned revision')
    parser.add_argument('--check', action='store_true', help='Verify committed JSON is reproducible')
    args = parser.parse_args()
    result = build(args.source)
    output = json.dumps(result, indent=2, ensure_ascii=False) + '\n'
    destination = CATALOG / 'bioscan.json'
    if args.check:
        if destination.read_text() != output:
            raise SystemExit('Catalog differs from source; run importer without --check')
    else:
        destination.write_text(output)
    print(f'{len(result["sourceFiles"])} files, {len(result["species"])} unique species, '
          f'{sum(len(s["rules"]) for s in result["species"])} rules; '
          f'{len(result["bodyTypes"])} body types. Catalog {"verified" if args.check else "written"}.')


if __name__ == '__main__':
    main()
