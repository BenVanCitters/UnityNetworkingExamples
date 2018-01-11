#!/usr/bin/env python

import tornado.ioloop
import tornado.web
import tornado.websocket
import argparse
import logging
import sys
import json
import random

from time import time

logger = logging.getLogger()
logger.setLevel(logging.DEBUG)
logger.addHandler(logging.StreamHandler(sys.stdout))

class TornadoPollingHandler(tornado.websocket.WebSocketHandler):

    def initialize(self, description):
        logger.info("initi polling")
        self.description = description
        self.reporter = tornado.ioloop.PeriodicCallback(self.send_report, 1000)

    def check_origin(self, origin):
        return True

    def open(self):
        logger.info("Test service connection opened")
        self.reporter.start()

    def on_close(self):
        if self.reporter and self.reporter.is_running():
            self.reporter.stop()
        logger.info("Test service connection closed")

    def send_report(self):
        data = { 'description': self.description }
        message = json.dumps(data)
        self.write_message(message)

    def on_message(self, message):
        self.write_message("You said: " + message)

class TornadoServiceHandler(tornado.web.RequestHandler):
    def initialize(self, random_value):
        logger.info("initi TornadoServiceHandler")
        self.random_value = random_value

    def get(self):
        message = json.dumps(self.get_data())
        self.write(message)

    def get_data(self):
        return { 'random_value': self.random_value }


def main():
    #handle passed in arguments
    parser = argparse.ArgumentParser(description='Raspberry Pi Tornado Web Socket Server')
    parser.add_argument('-i', '--interface', dest='interface', default='0.0.0.0')
    parser.add_argument('-p', '--port', dest='port', type=int, default=9999)
    args = parser.parse_args()

    #setup the tornado app

    app = tornado.web.Application([
        (r'/service', TornadoServiceHandler, { 'random_value': random.uniform(1.5, 1.9)}),
        (r'/pollingservice', TornadoPollingHandler, { 'description': ""})
    ])

    app.listen(args.port, address=args.interface)
    #start the tornado server
    tornado.ioloop.IOLoop.instance().start()


if __name__ == '__main__':
    try:
        logger.info("Starting Raspberry Pi Tornado Web Socket server\n")
        main()
    except KeyboardInterrupt as e:
        logger.info("\nRaspberry Pi Tornado Web Socket server interruptted")
