create or replace function setsessiondata(sessionid bigint, datakey text, datavalue text, datatype text) returns SETOF "SessionData"
    language plpgsql
as
$$
BEGIN
    PERFORM * FROM "Sessions" WHERE "SessionId" = sessionid;
    IF FOUND
    THEN
        FOUND := FALSE;
        PERFORM *
        FROM "SessionData"
        WHERE "SessionId" = sessionid
          AND "DataKey" = datakey;
        IF FOUND
        THEN
            UPDATE "SessionData"
            SET "DataValue" = datavalue,
                "DataType"  = datatype
            WHERE "SessionId" = sessionid
              AND "DataKey" = datakey;
        ELSE
            INSERT INTO "SessionData" ("SessionId", "DataKey", "DataType", "DataValue") VALUES (sessionid, datakey, datatype, datavalue);
        END IF;

        RETURN QUERY SELECT * FROM GetSessionData(sessionid, datakey);
    ELSE
        RAISE EXCEPTION 'No Session with ID % was found', sessionid;
    END IF;
END
$$;

