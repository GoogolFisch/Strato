
import socket
import sys
import threading

so = socket.socket(socket.AF_INET,socket.SOCK_STREAM)
PORT = 25359
CHAT_PACKET = 2

so.connect(("127.0.0.1",PORT))

name = input("name?>>")
bname = bytes(name,"utf-8")

stopping = False
bbuf = b""

def bytes2int(bts,idx):
    return bts[idx] + (bts[idx + 2] << 8) + (bts[idx + 2] << 16) + (bts[idx + 3] << 24)

def rec():
    while not stopping:
        buffer = bbuf + so.recv(1024)
        idx = 0
        while not stopping:
            if(len(buffer) < 8 + idx):break
            lng = bytes2int(buffer,idx + 0)
            id_ = bytes2int(buffer,idx + 4)
            if(len(buffer) < idx + 8 + lng):break
            idx += 8
            if(id_ != CHAT_PACKET):
                idx += lng
                continue
            #
            nlen = bytes2int(buffer,idx + 0)
            name = buffer[idx + 4:idx + 4 + nlen]
            idx += 4 + nlen
            mlen = bytes2int(buffer,idx + 0)
            msg = buffer[idx + 4:idx + 4 + mlen]
            idx += 4 + mlen
            print(str(name,"utf-8"),str(msg,"utf-8"))


        
th1 = threading.Thread(target=rec)


def int2bytes(num:int):
    return bytes(bytearray([ num & 0xff,
                    (num >> 8) & 0xff,
                    (num >> 16) & 0xff,
                    (num >> 24) & 0xff]))

def raw2byte(msg:str):
    bmsg = bytes(msg,"utf-8")
    data = int2bytes(len(bname)) + bname + int2bytes(len(bmsg)) + bmsg
    return int2bytes(len(data)) + int2bytes(CHAT_PACKET) + data

th1.start()
try:
    while not stopping:
        inp = input(">>")
        so.send(raw2byte(inp))
except Exception as e:
    print(e)
    stopping = True
    th1.join()
except KeyboardInterrupt as e:
    stopping = True
    th1.join()


sys.exit(0)
