DROP DATABASE IF EXISTS freeml;
CREATE DATABASE freeml;

\connect freeml

CREATE TABLE jobs (
    id BIGINT PRIMARY KEY,
    name VARCHAR(30) NOT NULL,
    description VARCHAR(255) NOT NULL,
    status VARCHAR(10) NOT NULL,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP NOT NULL
);
