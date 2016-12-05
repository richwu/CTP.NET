CREATE TABLE CfgUserInserOrder (
    SubUserID  VARCHAR NOT NULL,
    MstUserID  VARCHAR NOT NULL,
    Instrument VARCHAR,
    Volume     DOUBLE  NOT NULL
                       DEFAULT (0),
    Price      DOUBLE  NOT NULL
                       DEFAULT (0),
    IsInverse  BOOLEAN NOT NULL
                       DEFAULT false
);