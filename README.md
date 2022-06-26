A generic data mapper, using the Description attribute on a POCO to manage the mappings.

The app depends on a SQLite db which is part of the repo. Not sure if you need SQLite installed locally, but it's a simple download and install from https://www.sqlite.org/download.html. All you do is unzip the file and put the extracted files into a folder.

The app uses reflection, which is historically "bad". However, I did some timings and while there are roughly order of magnitiude differences, the overall numbers we are looking at are small. Probably insignificant relative to the combined network latency and db processing time.

By default, the db is empty to start with so running the first time with no parameter will populate the db with 10,000 records.

On subsequent runs, specify a single integer as input to limit the number of rows retrieved and processed.

Let's run the numbers...
| Object | Records | Reflection [ms] | Classic [ms] |
| ------ | ------- | ---------------- | ------------ |
| Person | 100 | 0.0094220 | 0.0014703 |
| Person | 1000 | 0.0190742 | 0.0024859|
| Person | 2000 | 0.0304010 | 0.0035670|
| Person | 5000 | 0.0644257 | 0.0095289|
| Person | 10000 | 0.1249254 | 0.0124453|
| | | | |
| Company | 100 | 0.0018797 |0.0014798 |
| Company | 1000 |0.0151256 | 0.0029994|
| Company | 2000 | 0.0282020|0.0046168 |
| Company | 5000 | 0.0751933| 0.0089756|
| Company | 10000 |0.1573565  |0.0185161|
