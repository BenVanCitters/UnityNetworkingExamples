import socket

UDP_IP = "10.102.52.201"
UDP_PORT = 5006

print("Initializing Server...")
sock = socket.socket(socket.AF_INET, # Internet
                     socket.SOCK_DGRAM) # UDP
sock.bind((UDP_IP, UDP_PORT))
print("...Done... looping...")
while True:
    data, addr = sock.recvfrom(1024) # buffer size is 1024 bytes
    print "received message:", data
