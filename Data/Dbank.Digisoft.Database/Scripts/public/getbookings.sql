create or replace function getbookings() returns SETOF bookings
    language plpgsql
as
$$
BEGIN
    RETURN QUERY
    SELECT distinct b.* FROM bookings b WHERE b."Active";
END;
$$;

