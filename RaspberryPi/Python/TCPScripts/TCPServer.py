import argparse
import logging
import socketserver
import sys

logger = logging.getLogger()
logger.setLevel(logging.DEBUG)
logger.addHandler(logging.StreamHandler(sys.stdout))


class MyRequestHandlerClass(socketserver.BaseRequestHandler):
    def handle(self):
        # self.request is the TCP socket connected to the client
        self.data = self.request.recv(1024).strip()
        print("{} wrote:".format(self.client_address[0]))
        print(self.data)

        # just send back the same data, but upper-cased
        self.request.sendall(self.data.upper())


def main():
    # handle passed in arguments
    parser = argparse.ArgumentParser(description='Raspberry Pi TCP Server')
    parser.add_argument('-i', '--ipaddress', dest='ipaddress', default='10.102.52.122')
    parser.add_argument('-p', '--port', dest='port', type=int, default=9999)
    args = parser.parse_args()

    logger.info("Starting Raspberry Pi TCP Server - " + args.ipaddress + " at port " + str(args.port) + "\n")


    server = socketserver.TCPServer((args.ipaddress,args.port),MyRequestHandlerClass)
    server.serve_forever()

if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt as e:
        logger.info("\nRaspberry Pi TCP Server interruptted")
