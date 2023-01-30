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


def login(email: str, password: str):
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


def status_to_text(status: int):
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
    response = [(x['id'], x['name'], x['description'], status_to_text(x['status'])) for x in response]
    print(tabulate(response, headers=['ID', 'Name', 'Description', 'Status']))


def _main():
    parser = argparse.ArgumentParser(
        prog='FreeML Client',
        description='A command line client for OpenML'
    )
    parser.add_argument('--email', help='Email address to use', required=True)
    parser.add_argument('--password', help='Password to use', required=True)
    parser.add_argument('command', help='Subcommand to run')
    args = parser.parse_args()
    
    if args.command not in globals():
        fatal_error('Invalid command')
    
    email = args.email
    password = args.password

    # Authenticate
    if email is None or password is None:
        fatal_error("Invalid credentials.")
    
    token = login(email, password)

    # Run the command
    globals()[args.command](token)


if __name__ == '__main__':
    _main()