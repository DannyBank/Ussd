create or replace function getsession(sessionid bigint, appid text) returns SETOF "Sessions"
    language plpgsql
as
$$
BEGIN

    RETURN
        QUERY SELECT *
              FROM "Sessions" s
              WHERE s."SessionId" = sessionid
                AND s."AppId" = appid
                AND s."LastResponseTime" > now() - '5 mins'::interval;
END;
$$;

create or replace function getsession(msisdn text, dialogid text, appid text) returns SETOF "Sessions"
    language plpgsql
as
$$
BEGIN

    RETURN
        QUERY SELECT *
              FROM "Sessions" s
              WHERE (s."Msisdn" = msisdn AND s."DialogId" = dialogid AND s."AppId" = appid)
                AND s."Status" != 3
                AND s."LastResponseTime" > now() - '5 mins'::interval
              LIMIT 2;
END;
$$;

