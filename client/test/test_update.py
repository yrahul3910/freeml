import pickle
import os
import sys
import argparse
from io import StringIO

import torch
from torch.nn import Module, Sequential, Linear, ReLU, Softmax, Flatten
from torchvision.datasets import MNIST
from torchvision.transforms import ToTensor
from dotenv import load_dotenv, find_dotenv

from src.main import _login, submit, run_job


class Model(Module):
    def __init__(self):
        super().__init__()
        self.model = Sequential(
            Flatten(),
            Linear(784, 10),
            ReLU(),
            Linear(10, 10),
            Softmax()
        )
    
    def forward(self, x):
        return self.model(x)


if __name__ == '__main__':
    # Create a new model
    model = Model()
    model_script = torch.jit.script(model)
    model_script.save('model.pt')

    # Grab the MNIST dataset
    data = MNIST('../data', train=True, download=True, transform=ToTensor())

    with open('data.pt', 'wb') as f:
        pickle.dump(data, f)
    
    # Read user credentials from .env
    load_dotenv(find_dotenv())
    username = os.getenv('USERNAME')
    password = os.getenv('PASSWORD')

    # Login to the server
    if not username or not password:
        raise ValueError('Need to specify a username and password in .env')
    
    # Patch sys.stdout
    outfile = sys.stdout
    sys.stdout = output = StringIO()

    token = _login(username, password)

    submit(token, argparse.Namespace(
        model='model.pt',
        data='data.pt',
        name='Test',
        description='Test'
    ))

    # Now run the job
    output.seek(0)
    job_id = output.read().split('with id ')[1]
    sys.stdin = StringIO(job_id + '\n')
    run_job(token)
