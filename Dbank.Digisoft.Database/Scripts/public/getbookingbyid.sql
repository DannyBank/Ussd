create or replace function getbookingbyid(bookingid bigint) returns SETOF bookings
    language plpgsql
as
$$
BEGIN
    RETURN QUERY
    SELECT b.* FROM bookings b WHERE b."BookingId" = bookingId and b."Active";
END;
$$;

