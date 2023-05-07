create or replace function getabandonedsessions() returns SETOF "Sessions"
    language plpgsql
as
$$
BEGIN
    RETURN QUERY SELECT *
                 FROM "Sessions"
                 WHERE "LastResponseTime" < now() - '5 mins' :: interval
                   AND "EndDate" IS NULL
                   AND "Status" <> 3
                 LIMIT 100;
END;
$$;

