"""Execute the generated aggregate SQL against independent per-city fixtures.

SQLite is used only for portable COUNT/CASE/GROUP BY semantics, not as a
substitute for live MySQL integration or a performance benchmark.
"""
import sqlite3
from pathlib import Path

queries = (Path(__file__).parent / "bin/statistics-queries.sql").read_text().splitlines()
cities = [
    ([(1, 1), (2, 1), (3, 2)], [(10, 1), (11, 1), (12, 2)],
     [(1, 4, 1), (1, 2, 0), (2, 2, 1), (3, 8, 1)],
     [{1: (2, 1), 2: (1, 1)}, {1: (3, 2), 2: (0, 0)}, {1: (2, 1)}]),
    ([(1, 1), (2, 2)], [(10, 1), (11, 2)],
     [(1, 5, 1), (2, 2, 1)],
     [{1: (1, 1), 2: (1, 0)}, {1: (1, 1), 2: (1, 0)}, {1: (1, 1)}]),
]
totals = [{} for _ in queries]
checks = 0
for projects, people, states, expected in cities:
    with sqlite3.connect(":memory:") as db:
        db.executescript("CREATE TABLE microproject(MP_ID, Category); CREATE TABLE person(P_ID, ProjectId); CREATE TABLE microproject_state(ProjectId, State_ID, Current_State);")
        db.executemany("INSERT INTO microproject VALUES (?, ?)", projects)
        db.executemany("INSERT INTO person VALUES (?, ?)", people)
        db.executemany("INSERT INTO microproject_state VALUES (?, ?, ?)", states)
        for i, query in enumerate(queries):
            actual = {group: (all_count, funded) for _, all_count, funded, group in db.execute(query)}
            assert actual == expected[i], (actual, expected[i])
            checks += 1
            for group, counts in actual.items():
                previous = totals[i].get(group, (0, 0))
                totals[i][group] = tuple(a + b for a, b in zip(previous, counts))
        db.execute("DELETE FROM microproject")
        assert all(not list(db.execute(query)) for query in queries)
        checks += 1
assert totals == [{1: (3, 2), 2: (2, 1)}, {1: (4, 3), 2: (1, 0)}, {1: (3, 2)}]
print(f"PASS: {checks + 1} generated SQL checks; duplicate joins, overlapping city IDs, filters, empty results, and combined totals.")
