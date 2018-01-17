import argparse
import logging
import sys
import socket

logger = logging.getLogger()
logger.setLevel(logging.DEBUG)
logger.addHandler(logging.StreamHandler(sys.stdout))

def main():
    #handle passed in arguments
    parser = argparse.ArgumentParser(description='Raspberry Pi TCP Client')
    parser.add_argument('-i', '--ipaddress', dest='ipaddress', default='10.102.52.122')
    parser.add_argument('-p', '--port', dest='port', type=int, default=9999)
    args = parser.parse_args()

    logger.info("Starting Raspberry Pi TCP Client - " + args.ipaddress + " at port " + str(args.port) + "\n")


    ## https://wiki.python.org/moin/TcpCommunication
    BUFFER_SIZE = 1024
    #send faust's opening line
    MESSAGE = "I've studied now Philosophy And Jurisprudence, Medicine, And even, alas! Theology All through and through with ardour keen! Here now I stand, poor fool, and see I'm just as wise as formerly. Am called a Master, even Doctor too, And now I've nearly ten years through Pulled my students by their noses to and fro And up and down, across, about, And see there's nothing we can know! That all but burns my heart right out. True, I am more clever than all the vain creatures, The Doctors and Masters, Writers and Preachers; No doubts plague me, nor scruples as well. I'm not afraid of devil or hell. To offset that, all joy is rent from me. I do not imagine I know aught that's right; I do not imagine I could teach what might Convert and improve humanity. Nor have I gold or things of worth, Or honours, splendours of the earth. No dog could live thus any more! So I have turned to magic lore, To see if through the spirit's power and speech Perchance full many a secret I may reach, So that no more with bitter sweat I need to talk of what I don't know yet, So that I may perceive whatever holds The world together in its inmost folds, See all its seeds, its working power, And cease word-threshing from this hour. Oh, that, full moon, thou didst but glow Now for the last time on my woe, Whom I beside this desk so oft Have watched at midnight climb aloft. Then over books and paper here To me, sad friend, thou didst appear! Ah! could I but on mountain height Go onward in thy lovely light, With spirits hover round mountain caves, Weave over meadows thy twilight laves, Discharged of all of Learning's fumes, anew Bathe me to health in thy healing dew. Woe! am I stuck and forced to dwell Still in this musty, cursed cell? Where even heaven's dear light strains But dimly through the painted panes! Hemmed in by all this heap of books, Their gnawing worms, amid their dust, While to the arches, in all the nooks, Are smoke-stained papers midst them thrust, Boxes and glasses round me crammed, And instruments in cases hurled, Ancestral stuff around me jammed- That is your world! That's called a world! And still you question why your heart Is cramped and anxious in your breast? Why each impulse to live has been repressed In you by some vague, unexplained smart? Instead of Nature's living sphere In which God made mankind, you have alone, In smoke and mould around you here, Beasts' skeletons and dead men's bone. Up! Flee! Out into broad and open land! And this book full of mystery, From Nostradamus' very hand, Is it not ample company? The stars' course then you'll understand And Nature, teaching, will expand The power of your soul, as when One spirit to another speaks. 'Tis vain To think that arid brooding will explain The sacred symbols to your ken. Ye spirits, ye are hovering near; Oh, answer me if ye can hear!"

    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((args.ipaddress, args.port))
    s.sendall(MESSAGE.encode())
    data = s.recv(BUFFER_SIZE)
    s.close()

    print("received data:", data)


if __name__ == '__main__':
    try:
        main()
    except KeyboardInterrupt as e:
        logger.info("\nRaspberry Pi TCP Client interruptted")