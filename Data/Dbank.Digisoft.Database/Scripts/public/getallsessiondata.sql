create or replace function getallsessiondata(sessionid bigint) returns SETOF "SessionData"
    language plpgsql
as
$$
BEGIN
    PERFORM * FROM "Sessions" WHERE "SessionId" = sessionid;
    IF FOUND
    THEN
        RETURN QUERY SELECT *
                     FROM "SessionData"
                     WHERE "SessionId" = sessionid;
    ELSE
        RAISE EXCEPTION 'No Session with ID % was found', sessionid;
    END IF;
END;
$$;

