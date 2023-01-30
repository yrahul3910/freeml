import argparse
import os

import requests
from tabulate import tabulate

from utils import fatal_error, info


if 'FREEML_MODE' not in os.environ or os.environ['FREEML_MODE'] == 'dev':
    BASE_URL = 'https://localhost:5001'
else:
    pass
    # TODO: Set the prod URL


if 'localhost' in BASE_URL:
    verify_cert = False
else:
    verify_cert = True


def _login(email: str, password: str):
    """
    Authenticates the user.
    """

    response = requests.post(f'{BASE_URL}/api/v1/account/login', json={
        'email': email,
        'password': password
    }, verify=verify_cert)

    if response.status_code != 200:
        fatal_error('Invalid credentials')
    
    info('Successfully logged in.')
    return response.json()['access_token']


def _status_to_text(status: int):
    """
    Converts a status code to a human-readable string.
    """
    match status:
        case 0: return 'Pending'
        case 1: return 'Running'
        case 2: return 'Completed'


def list(token: str):
    """
    Lists available jobs, along with their descriptions.
    """
    response = requests.get(f'{BASE_URL}/api/v1/job', 
        headers={'Authorization': f'Bearer {token}'},
        verify=verify_cert)

    if response.status_code != 200:
        fatal_error('Failed to list jobs')

    response = response.json()
    response = [(x['id'], x['name'], x['description'], _status_to_text(x['status'])) for x in response]
    print(tabulate(response, headers=['ID', 'Name', 'Description', 'Status']))


def submit(token: str, args: argparse.Namespace):
    """
    Submits a job to the server.
    """
    if not args.model or not args.data:
        fatal_error('Need to specify a model and data')
    
    if not args.name or not args.description:
        fatal_error('Need to specify a name and description')
    
    if not os.path.exists(args.model):
        fatal_error('Model does not exist')
    
    if not os.path.exists(args.data):
        fatal_error('Data does not exist')
    
    # Check that the size of the model and data are under 47MB.
    # This is because the server has a 50MB limit, and we need to
    # account for the overhead of the request.
    if os.path.getsize(args.model) + os.path.getsize(args.data) > 47 * 1024 * 1024:
        fatal_error('Model and data are too large')
    
    data = {
        'name': args.name,
        'description': args.description,
        'status': 0
    }

    files = {
        'model': (args.model, open(args.model, 'rb')),
        'dataset': (args.data, open(args.data, 'rb'))
    }
    
    # Submit the job
    response = requests.post(f'{BASE_URL}/api/v1/job',
        headers={'Authorization': f'Bearer {token}'}, 
        data=data,
        files=files,
        verify=verify_cert)

    print(response.json())
    
    if response.status_code != 200:
        fatal_error('Failed to submit job')
    
    info('Successfully submitted job with id ' + str(response.json()['id']))


def _main():
    parser = argparse.ArgumentParser(
        prog='FreeML Client',
        description='A command line client for OpenML'
    )
    parser.add_argument('--email', help='Email address to use', required=True)
    parser.add_argument('--password', help='Password to use', required=True)
    parser.add_argument('--model', help='Model to use')
    parser.add_argument('--data', help='Data to use')
    parser.add_argument('--name', help='Name of the job')
    parser.add_argument('--description', help='Description of the job')
    parser.add_argument('command', help='Subcommand to run')
    args = parser.parse_args()
    
    if args.command not in globals():
        fatal_error('Invalid command')
    
    email = args.email
    password = args.password

    # Authenticate
    if email is None or password is None:
        fatal_error("Invalid credentials.")
    
    token = _login(email, password)

    # Run the command
    match args.command:
        case 'list': list(token)
        case 'submit': submit(token, args)


if __name__ == '__main__':
    _main()