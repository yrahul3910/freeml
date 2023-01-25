from colorama import Fore, Style


def warn(message: str) -> None:
    print(f'{Fore.YELLOW}[WARN]: {message}{Style.RESET_ALL}')


def error(message: str) -> None:
    print(f'{Fore.RED}[ERR]: {message}{Style.RESET_ALL}')


def info(message: str) -> None:
    print(f'{Fore.BLUE}[INFO]: {message}{Style.RESET_ALL}')


def fatal_error(message: str, code: int=1):
    print(f'{Fore.RED}[FATAL]: {message}{Style.RESET_ALL}')
    exit(code)