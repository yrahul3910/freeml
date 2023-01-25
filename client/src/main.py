import argparse
import os

import cutie
import requests
from tabulate import tabulate

from utils import fatal_error


if 'FREEML_MODE' not in os.environ or os.environ['FREEML_MODE'] == 'dev':
    BASE_URL = 'http://localhost:5000'
    # TODO: Set the prod URL


def list():
    """
    Lists available jobs, along with their descriptions.
    """
    response = requests.get(f'{BASE_URL}/jobs/list')
    print(tabulate(response.json(), headers=['Name', 'Description']))


def _main():
    parser = argparse.ArgumentParser(
        prog='FreeML Client',
        description='A command line client for OpenML'
    )
    parser.add_argument('command', help='Subcommand to run')
    args = parser.parse_args()

    if args.command not in locals():
        fatal_error('Invalid command')

    locals()[args.command]()


if __name__ == '__main__':
    _main()