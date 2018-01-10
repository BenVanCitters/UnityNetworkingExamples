import socket

UDP_IP = "10.102.52.122"
UDP_PORT = 9999
MESSAGE = "Bonjour, le monde!"

print( "UDP target IP:", UDP_IP)
print( "UDP target port:", UDP_PORT)
print( "message:", MESSAGE)

sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
sock.sendto(MESSAGE.encode('utf-8'), (UDP_IP, UDP_PORT))
