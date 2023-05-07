create or replace function createsession(msisdn text, dialogid text, appid text, sessionid bigint) returns SETOF "Sessions"
    language plpgsql
as
$$
BEGIN
    RETURN QUERY
        INSERT INTO "Sessions" ("Msisdn", "DialogId", "AppId", "Page", "Stage", "Status", "SessionId")
            VALUES (msisdn, dialogid, appid, 1, 0, 1, sessionid)
            RETURNING *;
END;
$$;

