create or replace function getcurrentsessionscountfortoday() returns bigint
    language plpgsql
as
$$
DECLARE
    sesscount bigint;
BEGIN
    SELECT Count(*)
    INTO sesscount
    FROM "Sessions"
    WHERE "StartTime" > now() - '5 min'::interval
      AND "Status" < 3;
    RETURN sesscount;
END;
$$;

