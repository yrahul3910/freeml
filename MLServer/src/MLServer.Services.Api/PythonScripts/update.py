import torch
import sys


def update(model_path: str, updates_path: str) -> None:
    LEARNING_RATE = 0.1
    model = torch.load(model_path)

    with open(updates_path, 'rb') as f:
        updates = f.read()

    with torch.no_grad():
        for name, param in model.named_parameters():
            if name not in updates:
                raise ValueError('Parameter not found in updates dict.')

            param -= param.grad * LEARNING_RATE

        model.zero_grad()

    with open(model_path, 'wb') as f:
        torch.save(model, f)


if __name__ == '__main__':
    if len(sys.argv) != 2:
        print(f'Usage: {sys.argv[0]} ID')
        sys.exit(1)

    _id = sys.argv[1]
    model_path = f'./jobs/{_id}/model'
    updates_path = f'./jobs/{_id}/update.bin'

    update(model_path, updates_path)