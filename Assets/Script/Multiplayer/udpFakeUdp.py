
import socket
import time
import sys

usock = socket.socket(socket.AF_INET,socket.SOCK_DGRAM)
usock.setsockopt(socket.SOL_SOCKET,socket.SO_BROADCAST,1)
PORT = 25359
DTIME = 15

def sending(data:bytes):
    counting = 0
    while True:
        print(end=f"{counting}\b\b\b\b\b",flush=True)
        counting += 1
        usock.sendto(data,("127.0.0.1",PORT))
        #/usock.sendto(data,("192.168.178.255",PORT))
        time.sleep(DTIME)

try:
    bv = b"Hello"
    if(len(sys.argv) > 1):
        bv = bytes(sys.argv[1],"utf-8")
    byting = b"\1\0\0\0"
    byting += bytes([len(bv) & 0xff,(len(bv) >> 8) & 0xff,(len(bv) >> 16) & 0xff,0])
    byting += bv
    sending(byting + b"\2\0\0\0\5\0\0\0");
except KeyboardInterrupt:
    pass
