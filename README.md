A generic data mapper, using the Description attribute on a POCO to manage the mappings.

The app depends on a Sql Server db. Scripts for the two tables are  part of the repo. 

The app uses reflection, which is historically "bad". However, I did some timings and while there are roughly sub-order of magnitiude differences, the overall numbers we are looking at are small. Probably insignificant relative to the combined network latency and db processing time.

By default, the db is empty to start with so running the first time with no parameter will populate the db with 10,000 records.

On subsequent runs, specify a single integer as input to limit the number of rows retrieved and processed.

Let's run the numbers...
| Object | Records | Reflection (R) [ms] | Classic (C) [ms] | R:C Ratio | 
| ------ | ------- | ---------------- | ------------ | --------- |
| Person | 100 | 0.16568 | 0.06590 | 2.5 |
| Person | 1000 | 1.59745 | 0.65932| 2.4 |
| Person | 2000 | 3.22925 | 1.46057 | 2.2 |
| Person | 5000 | 8.37891 | 3.23607 | 2.6 |
| Person | 10000 | 18.17499 | 8.22381 | 2.2 |
| Person | 25000 | 43.17623 | 25.51766 | 1.7 |
| Person | 50000 | 80.30633 | 41.60873 | 1.9 |
| Person | 100000 | 159.64431 | 89.899899 | 1.8 |
| | | | | |
| Company | 100 | 0.23333 |0.07928 | 2.9 |
| Company | 1000 | 2.01644 | 0.76209 | 2.6 |
| Company | 2000 | 4.11572 | 1.53632| 2.7 |
| Company | 5000 | 10.56675 | 3.82804 | 2.8 |
| Company | 10000 | 22.87474 | 9.18897 | 2.5 |
| Company | 25000 | 51.51073 | 21.28270 | 2.4 |
| Company | 50000 | 99.22539 | 46.96870 | 2.1 |
| Company | 100000 | 207.76768 | 105.70928 | 2.0|

