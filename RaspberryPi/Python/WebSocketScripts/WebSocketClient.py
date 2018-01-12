#!/usr/bin/env python
# -*- coding: utf-8 -*-

from tornado.ioloop import IOLoop, PeriodicCallback
from tornado import gen
from tornado.websocket import websocket_connect

import sys
import argparse
import logging

logger = logging.getLogger()
logger.setLevel(logging.DEBUG)
logger.addHandler(logging.StreamHandler(sys.stdout))

class Client(object):
    #constructor
    def __init__(self, url, timeout):
        self.url = url
        self.timeout = timeout
        self.ioloop = IOLoop.instance()
        self.ws = None
        self.connect()
        PeriodicCallback(self.keep_alive, 20000, io_loop=self.ioloop).start()
        self.ioloop.start()

    @gen.coroutine
    def connect(self):
        logger.info("Trying to connect to " + self.url)
        try:
            self.ws = yield websocket_connect(self.url)
        except Exception as e:
            logger.info("Connection error " + e)
        else:
            logger.info("Connected to " + self.url)
            self.send_msg()
            self.run()

    @gen.coroutine
    def run(self):
        while True:
            msg = yield self.ws.read_message()
            if msg is None:
                logger.info("Client connection closed")
                self.ws = None
                break

    def send_msg(self):
        self.ws.write_message("here's an unformated string message")


    #callback the gets re-called every so often to make sure the connection stays open
    def keep_alive(self):
        if self.ws is None:
            self.connect()
        else:
            self.ws.write_message("keep alive")


if __name__ == "__main__":
    # handle passed in arguments
    parser = argparse.ArgumentParser(description='Raspberry Pi Tornado Web Socket Client')
    parser.add_argument('-i', '--ipaddress', dest='ipaddress', default='0.0.0.0')
    parser.add_argument('-p', '--port', dest='port', type=int, default=9999)
    parser.add_argument('-t', '--timeout', dest='timeout', type=int, default=5)
    args = parser.parse_args()

    url_with_port = "ws://" + args.ipaddress + ":" + str(args.port) + "/service"
    logger.info("intializing the client with url: " + url_with_port)
    client = Client(url_with_port, args.timeout)
