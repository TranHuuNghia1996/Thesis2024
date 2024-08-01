﻿-- using 1365545250 as a seed to the RNG


SELECT
	L_ORDERKEY,
	SUM(L_EXTENDEDPRICE * (1 - L_DISCOUNT)) AS REVENUE,
	O_ORDERDATE,
	O_SHIPPRIORITY
FROM
	CUSTOMER,
	ORDERS,
	LINEITEM
WHERE
	C_MKTSEGMENT = 'AUTOMOBILE'
	AND C_CUSTKEY = O_CUSTKEY
	AND L_ORDERKEY = O_ORDERKEY
	AND O_ORDERDATE < DATE '1995-03-13'
	AND L_SHIPDATE > DATE '1995-03-13'
GROUP BY
	L_ORDERKEY,
	O_ORDERDATE,
	O_SHIPPRIORITY
ORDER BY
	REVENUE DESC,
	O_ORDERDATE
LIMIT 10;
