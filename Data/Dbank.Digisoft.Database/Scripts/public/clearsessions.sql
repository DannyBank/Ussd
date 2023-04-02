create or replace function clearsessions() returns void
    language plpgsql
as
$$
BEGIN
    DELETE FROM "Sessions"
    WHERE "StartTime" < now() - '3 months'::interval;
END
$$;

