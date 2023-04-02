create table if not exists "SessionData"
(
    "SessionId" bigint not null
        references "Sessions"
            on update cascade on delete cascade,
    "DataKey"   text   not null,
    "DataType"  text   not null,
    "DataValue" text   not null
);

create index if not exists sessiondata_idkey_ix
    on "SessionData" ("SessionId", "DataKey");

