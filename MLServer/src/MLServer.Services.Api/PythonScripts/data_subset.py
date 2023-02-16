import sys
import pickle

from torch.utils.data import Dataset, random_split


def make_subset(data_path: str, output_path: str) -> None:
    with open(data_path, 'rb') as f:
        data = pickle.load(f)

    assert isinstance(data, Dataset)
    subset_size = int(0.2 * len(data))

    subset, _ = random_split(data, [subset_size, len(data) - subset_size])

    with open(output_path, 'wb') as f:
        pickle.dump(subset, f)


if __name__ == '__main__':
    if len(sys.argv) != 2:
        print(f'Usage: {sys.argv[0]} BASE_PATH')
        sys.exit(1)

    base_path = sys.argv[1]
    data_path = f'{base_path}/data'
    output_path = f'{base_path}/subset'

    make_subset(data_path, output_path)