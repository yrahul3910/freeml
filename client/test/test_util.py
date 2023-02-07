from torch.nn import Sequential, Linear, ReLU, Softmax, Flatten
from torch.optim import Adam
from torchvision.datasets import MNIST
from torchvision.transforms import ToTensor

from src.utils import gradient_update


def test_gradient_update():
    model = Sequential(
        Flatten(),
        Linear(784, 10),
        ReLU(),
        Linear(10, 10),
        Softmax()
    )

    data = MNIST('../data', train=True, download=True, transform=ToTensor())
    update = gradient_update(model, data)

    assert isinstance(update, dict)
    assert len(update) > 0