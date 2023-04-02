create table if not exists "Sessions"
(
    "SessionId"        bigint                  not null
        primary key,
    "Msisdn"           text                    not null,
    "DialogId"         text                    not null,
    "AppId"            text                    not null,
    "Page"             integer                 not null,
    "Stage"            integer                 not null,
    "Status"           integer                 not null,
    "StartTime"        timestamp default now() not null,
    "EndDate"          timestamp,
    "LastResponseTime" timestamp default now()
);

create index if not exists sessions_msisdn_ix
    on "Sessions" ("Msisdn");

create index if not exists sessions_msisdn_dialogid_appid_index
    on "Sessions" ("Msisdn", "DialogId", "AppId");

create index if not exists sessions_start_desc_index
    on "Sessions" ("StartTime" desc);

