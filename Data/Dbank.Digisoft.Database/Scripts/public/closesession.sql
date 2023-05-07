create or replace function closesession(sessionid bigint) returns SETOF "Sessions"
    language plpgsql
as
$$
BEGIN
    PERFORM * FROM "Sessions" WHERE "SessionId" = sessionid;
    IF FOUND
    THEN
        UPDATE "Sessions"
        SET "Status"           = 3,
            "EndDate"          = now(),
            "LastResponseTime" = now()
        WHERE "SessionId" = sessionid;

        RETURN QUERY SELECT * FROM "Sessions" WHERE "SessionId" = sessionid;
    ELSE
        RAISE EXCEPTION 'No Session with ID % was found', sessionid;
    END IF;
END;
$$;

