# OpenML

OpenML is about democratizing AI through decentralization. The goal is to have a server that can accept ML jobs (architectures and datasets) and distribute them to multiple volunteer nodes. The nodes will then train the models and send the results back to the server. The server will then aggregate the results via federated learning and send the new model back to the nodes. This process will repeat until the model converges.

## Getting Started

### Setting up the server

To set up the server:

1. Install [Postgres](https://www.postgresql.org/download/) and create a database.
2. Set up Postgres, and set the username and DB name in `server/.env`.

```
DB_NAME=<dbname>
DB_USER=<username>
```