import os
import random
import hashlib
from datetime import datetime

import psycopg
from dotenv import load_dotenv
from flask import Flask, request, Response


app = Flask(__name__)

load_dotenv()
DB_NAME = os.getenv('DB_NAME')
DB_USER = os.getenv('DB_USER')


@app.route('/jobs/list')
def list_jobs():
    """
    Returns a list of all jobs in the database
    """
    with psycopg.connect(f'dbname={DB_NAME} user={DB_USER}') as conn:
        with conn.cursor() as cur:
            cur.execute('SELECT * FROM jobs')
            jobs = cur.fetchall()

            # Just get the names and descriptions
            # TODO: Fix--I don't think this actually works
            jobs = [([x[0], x[1]]) for x in jobs]
            return Response(jobs, status=200)


@app.route('/jobs/submit', methods=['POST'])
def submit_job():
    """
    Accepts a job submission and returns a job ID. The request must contain
    {
        "name": string,
        "description": string,
        "dataset": bytes?,
        "model": bytes?,
        "model_hash": string,
        "dataset_hash": string
    }
    """
    name, description, dataset, model = request['name'], request['description'], request['dataset'], request['model']

    # Check SHA256 hashes
    if (hashlib.sha256(dataset).hexdigest() != request['dataset_hash'] or
        hashlib.sha256(model).hexdigest() != request['model_hash']):
        return Response('Hashes do not match', status=400)

    job_id = random.randint(0, 1000000)

    # Make sure it doesn't already exist
    while os.path.exists(f'jobs/{job_id}'):
        job_id = random.randint(0, 1000000)
    
    # Then create it
    os.mkdir(f'jobs/{job_id}')

    # Move the model and dataset into the job directory
    with open(f'jobs/{job_id}/dataset', 'wb') as f:
        f.write(dataset)
    
    with open(f'jobs/{job_id}/model', 'wb') as f:
        f.write(model)

    # Write details to a SQL database
    with psycopg.connect(f'dbname={DB_NAME} user={DB_USER}') as conn:
        with conn.cursor() as cur:
            cur.execute('INSERT INTO jobs (id, name, description, status, created_at, updated_at) VALUES (%s, %s, %s, %s, %s, %s)', 
                (job_id, name, description, 'pending', str(datetime.now()), str(datetime.now())))

            conn.commit()

            return Response(job_id, status=200)
