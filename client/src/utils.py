from typing import Dict

from colorama import Fore, Style
import torch
from torch.nn import Module, CrossEntropyLoss
from torch.utils.data import TensorDataset, DataLoader


def warn(message: str) -> None:
    print(f'{Fore.YELLOW}[WARN]: {message}{Style.RESET_ALL}')


def error(message: str) -> None:
    print(f'{Fore.RED}[ERR]: {message}{Style.RESET_ALL}')


def info(message: str) -> None:
    print(f'{Fore.BLUE}[INFO]: {message}{Style.RESET_ALL}')


def fatal_error(message: str, code: int=1):
    print(f'{Fore.RED}[FATAL]: {message}{Style.RESET_ALL}')
    exit(code)


def gradient_update(model: Module, data: TensorDataset, n_epochs: int) -> Dict[str, torch.Tensor]:
    """
    Runs a gradient update on the given model and data.
    """
    BATCH_SIZE = 128
    LEARNING_RATE = 0.1
    LOSS_FUNC = CrossEntropyLoss()

    train_dl = DataLoader(data, batch_size=BATCH_SIZE)
    update = {}

    for _ in range(n_epochs):
        for xb, yb in train_dl:
            pred = model(xb)
            loss = LOSS_FUNC(pred, yb)

            loss.backward()
            with torch.no_grad():
                for name, param in model.named_parameters():
                    if name not in update:
                        update[name] = torch.zeros_like(param.grad)

                    update[name] += param.grad * LEARNING_RATE
                    param -= param.grad * LEARNING_RATE
                
                model.zero_grad()
    
    return update