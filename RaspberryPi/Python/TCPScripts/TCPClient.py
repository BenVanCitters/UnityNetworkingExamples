import argparse
import logging

logger = logging.getLogger()
logger.setLevel(logging.DEBUG)
logger.addHandler(logging.StreamHandler(sys.stdout))

def main():
    #handle passed in arguments
    parser = argparse.ArgumentParser(description='Raspberry Pi TCP Client')
    parser.add_argument('-i', '--ipaddress', dest='ipaddress', default='0.0.0.0')
    parser.add_argument('-p', '--port', dest='port', type=int, default=9999)
    args = parser.parse_args()

    logger.info("Starting Raspberry Pi TCP Client - " + args.interface + " at port " + str(args.port) + "\n")

if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt as e:
        logger.info("\nRaspberry Pi TCP Client interruptted")