import unittest
from src.server import app


class TestServer(unittest.TestCase):
    def setUp(self):
        self.app = app.test_client()

    def test_list_jobs(self):
        response = self.app.get('/jobs/list')
        self.assertEqual(response.status_code, 200)


if __name__ == '__main__':
    unittest.main()