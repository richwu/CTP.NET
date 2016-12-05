CREATE TABLE CtpUser (
    UserID   VARCHAR PRIMARY KEY
                     NOT NULL,
    UserName STRING  NOT NULL,
    Password VARCHAR,
    BrokerID VARCHAR NOT NULL
                     REFERENCES CtpBroker (BrokerID),
    IsSub    BOOLEAN NOT NULL
                     DEFAULT false
);
