import torch
import pickle
import sys


def update(model_path: str, updates_path: str) -> None:
    LEARNING_RATE = 0.1
    model = torch.jit.load(model_path)

    with open(updates_path, 'rb') as f:
        updates = pickle.load(f)

    with torch.no_grad():
        for name, param in model.named_parameters():
            param -= updates[name]

        model.zero_grad()

    with open(model_path, 'wb') as f:
        torch.jit.save(model, f)


if __name__ == '__main__':
    if len(sys.argv) != 2:
        print(f'Usage: {sys.argv[0]} BASE_PATH')
        sys.exit(1)

    base_path = sys.argv[1]
    model_path = f'{base_path}/model'
    updates_path = f'{base_path}/update.bin'

    update(model_path, updates_path)