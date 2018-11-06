CREATE TABLE b_route (
    route_id  INTEGER      PRIMARY KEY ASC AUTOINCREMENT
                           NOT NULL,
    roue_name STRING (255),
    active    STRING (255),
    remark    STRING (255) 
);

CREATE TABLE b_staff (
    staff_id   INTEGER      PRIMARY KEY ASC AUTOINCREMENT
                            NOT NULL,
    staff_name STRING (255),
    active     STRING (255),
    remark     STRING (255) 
);

CREATE TABLE b_location (
    loca_id   BIGINT       PRIMARY KEY
                           NOT NULL,
    loca_name STRING (255),
    active    STRING (255),
    remark    STRING (255) 
);

PRAGMA foreign_keys = 0;

CREATE TABLE sqlitestudio_temp_table AS SELECT *
                                          FROM b_location;

DROP TABLE b_location;

CREATE TABLE b_location (
    loca_id   INTEGER      PRIMARY KEY ASC AUTOINCREMENT
                           NOT NULL,
    loca_name STRING (255),
    active    STRING (255),
    remark    STRING (255) 
);

INSERT INTO b_location (
                           loca_id,
                           loca_name,
                           active,
                           remark
                       )
                       SELECT loca_id,
                              loca_name,
                              active,
                              remark
                         FROM sqlitestudio_temp_table;

DROP TABLE sqlitestudio_temp_table;

PRAGMA foreign_keys = 1;


CREATE TABLE b_place (
    place_id   INTEGER      PRIMARY KEY ASC AUTOINCREMENT,
    place_name STRING (255),
    active     STRING (255),
    remark     STRING (255) 
);


PRAGMA foreign_keys = 0;

CREATE TABLE sqlitestudio_temp_table AS SELECT *
                                          FROM b_route;

DROP TABLE b_route;

CREATE TABLE b_route (
    rou_id   INTEGER      PRIMARY KEY ASC AUTOINCREMENT
                          NOT NULL,
    rou_name STRING (255),
    active   STRING (255),
    remark   STRING (255) 
);

INSERT INTO b_route (
                        rou_id,
                        rou_name,
                        active,
                        remark
                    )
                    SELECT route_id,
                           roue_name,
                           active,
                           remark
                      FROM sqlitestudio_temp_table;

DROP TABLE sqlitestudio_temp_table;

PRAGMA foreign_keys = 1;

CREATE INDEX "" ON b_location (
    loca_id,
    loca_name,
    active
);


CREATE TABLE t_count (
    count_id   INTEGER      PRIMARY KEY ASC AUTOINCREMENT,
    count_date STRING (255),
    staff_name STRING (255),
    rou_name   STRING (255),
    loca_name  STRING (255),
    place_name STRING (255),
    c1         STRING (255),
    c2         STRING (255),
    c5         STRING (255),
    c10        STRING (255),
    b20        STRING (255),
    b50        STRING (255),
    b100       STRING (255),
    b500       STRING (255),
    b1000      STRING (255) 
);

PRAGMA foreign_keys = 0;

CREATE TABLE sqlitestudio_temp_table AS SELECT *
                                          FROM t_count;

DROP TABLE t_count;

CREATE TABLE t_count (
    count_id   INTEGER      PRIMARY KEY ASC AUTOINCREMENT,
    count_date STRING (255),
    staff_name STRING (255),
    rou_name   STRING (255),
    loca_name  STRING (255),
    place_name STRING (255),
    c1         STRING (255),
    c2         STRING (255),
    c5         STRING (255),
    c10        STRING (255),
    b20        STRING (255),
    b50        STRING (255),
    b100       STRING (255),
    b500       STRING (255),
    b1000      STRING (255),
    active     STRING (255),
    remark     STRING (255),
    plus1      STRING (255),
    minus1     STRING (255) 
);

INSERT INTO t_count (
                        count_id,
                        count_date,
                        staff_name,
                        rou_name,
                        loca_name,
                        place_name,
                        c1,
                        c2,
                        c5,
                        c10,
                        b20,
                        b50,
                        b100,
                        b500,
                        b1000
                    )
                    SELECT count_id,
                           count_date,
                           staff_name,
                           rou_name,
                           loca_name,
                           place_name,
                           c1,
                           c2,
                           c5,
                           c10,
                           b20,
                           b50,
                           b100,
                           b500,
                           b1000
                      FROM sqlitestudio_temp_table;

DROP TABLE sqlitestudio_temp_table;

PRAGMA foreign_keys = 1;

CREATE TABLE b_comport (
    id        INTEGER      PRIMARY KEY ASC AUTOINCREMENT,
    baud_rate STRING (255),
    parity    STRING (255),
    stop_bit  STRING (255),
    data_bit  STRING (255) 
);
