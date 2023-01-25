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

INSERT INTO jobs VALUES (1, 'job1', 'description1', 'running', '2017-01-01 00:00:00', '2017-01-01 00:00:00');
INSERT INTO jobs VALUES (2, 'job2', 'description2', 'pending', '2017-01-01 00:00:00', '2017-01-01 00:00:00');