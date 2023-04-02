create table if not exists bookings
(
    "BookingId"   bigserial,
    "Description" text    not null,
    "Code"        text    not null,
    "Home"        text    not null,
    "Away"        text    not null,
    "Prediction"  text    not null,
    "CreateDate"  timestamp,
    "Active"      boolean not null,
    "BookingName" text
);

