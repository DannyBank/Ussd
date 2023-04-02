create or replace function updatesession(sessionid bigint, stage integer, page integer) returns SETOF "Sessions"
    language plpgsql
as
$$
BEGIN
    PERFORM * FROM "Sessions" WHERE "SessionId" = sessionid;
    IF FOUND
    THEN
        UPDATE "Sessions"
        SET "Stage"            = stage,
            "Page"             = page,
            "LastResponseTime" = now()
        WHERE "SessionId" = sessionid;
        RETURN QUERY SELECT * FROM "Sessions" WHERE "SessionId" = sessionid;
    ELSE
        RAISE EXCEPTION 'No Session with ID % was found', sessionid;
    END IF;
END;
$$;

create or replace function updatesession(sessionid bigint, dialogid text, stage integer, page integer) returns SETOF "Sessions"
    language plpgsql
as
$$
BEGIN
    PERFORM * FROM "Sessions" WHERE "SessionId" = sessionid;
    IF FOUND
    THEN
        UPDATE "Sessions"
        SET "Stage"           = stage,
            "Page"            = page,
            "LastResponseTime"= now()
        WHERE "SessionId" = sessionid;
        RETURN QUERY SELECT * FROM "Sessions" WHERE "SessionId" = sessionid;
    ELSE
        RAISE EXCEPTION 'No Session with ID % was found', sessionid;
    END IF;
END;
$$;

