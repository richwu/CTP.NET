CREATE TABLE CtpBroker (
    BrokerID           VARCHAR PRIMARY KEY
                               NOT NULL,
    BrokerName         STRING  NOT NULL,
    TraderFrontAddress VARCHAR,
    MarketFrontAddress VARCHAR
);