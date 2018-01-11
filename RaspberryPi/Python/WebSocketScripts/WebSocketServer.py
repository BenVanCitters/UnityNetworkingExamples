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
        logger.info("Initializing TornadoPollingHandler")
        self.description = description
        #setup a 1 second looping message to connected clients
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

    #send off json'ified dictionary
    def send_report(self):
        data = { 'description': self.description }
        message = json.dumps(data)
        self.write_message(message)

    #pass back anything that is passed to us
    def on_message(self, message):
        logger.info("Message Recieved: " + message + " sending back")
        self.write_message("You said: " + message)


def main():
    #handle passed in arguments
    parser = argparse.ArgumentParser(description='Raspberry Pi Tornado Web Socket Server')
    parser.add_argument('-i', '--interface', dest='interface', default='0.0.0.0')
    parser.add_argument('-p', '--port', dest='port', type=int, default=9999)
    args = parser.parse_args()

    #setup the tornado app

    app = tornado.web.Application([
        (r'/pollingservice_marx', TornadoPollingHandler, {'description': "A commodity is, in the first place, an object outside us, a thing that by its properties satisfies human wants of some sort or another. The nature of such wants, whether, for instance, they spring from the stomach or from fancy, makes no difference. Neither are we here concerned to know how the object satisfies these wants, whether directly as means of subsistence, or indirectly as means of production. -Marx"}),
        (r'/pollingservice_darwin', TornadoPollingHandler, { 'description': "When we look to the individuals of the same variety or sub-variety of our older cultivated plants and animals, one of the first points which strikes us, is, that they generally differ much more from each other, than do the individuals of any one species or variety in a state of nature. - Darwin"})
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
