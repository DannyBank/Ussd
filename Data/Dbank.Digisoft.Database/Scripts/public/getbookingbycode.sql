create or replace function getbookingbycode(code text) returns SETOF bookings
    language plpgsql
as
$$
BEGIN
    RETURN QUERY
    SELECT b.* FROM bookings b WHERE b."Code" = code and b."Active";
END;
$$;

