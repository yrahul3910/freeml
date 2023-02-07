# FreeML

FreeML is about democratizing AI through decentralization. The goal is to have a server that can accept ML jobs (architectures and datasets) and distribute them to multiple volunteer nodes. The nodes will then train the models and send the results back to the server. The server will then aggregate the results via federated learning and send the new model back to the nodes. This process will repeat until the model converges.

## Getting Started

### Setting up the server

To set up the server:

1. Install [Postgres](https://www.postgresql.org/download/) and create a database.
2. Set up Postgres, and set the username and DB name in `server/.env`.

```
DB_NAME=<dbname>
DB_USER=<username>
```

Set up the database:

```
psql -d "dbname='freeml' user=<username> password=<password> host='localhost'" -f setup-dev.sql
```

In `MLServer.Services.Api`, update `appsettings.json` to reflect the correct database connection string. You may need to log in to `psql` and create a database called `freeml` first.

### Setting up the CLI client

Python 3.10 is recommended. The code uses Python 3.10's `match` feature. Python 3.11 is currently [unsupported by PyTorch](https://github.com/pytorch/pytorch/issues/86566). For a prod deployment, set the `FREEML_MODE` environment variable to `prod`.

## Usage

### CLI

To submit a new job, run

```
python3.10 main.py --email='user@test.com' --password='password' --model='./model.pkl' --data='.data.pkl' --name='test' --description='description' submit
```

To list all jobs, run

```
python3.10 main.py --email='user@test.com' --password='password' list
```

Alternatively, you can run it in interactive mode:

```
python3.10 main.py --email='user@test.com' --password='password'
```

## Maintenance

### EF Core Migrations

To run migrations, run

```
dotnet ef migrations add {MigrationName} --context ApplicationDbContext --startup-project ../MLServer.Services.Api
dotnet ef database update --context ApplicationDbContext --startup-project ../MLServer.Services.Api
```
