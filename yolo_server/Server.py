import asyncio
import orjson
import socket
from enum import IntEnum, auto
from Car_detection import Car_Detection

async def handler(reader, writer):
    addr = writer.get_extra_info('peername')
    print(f"Connection from {addr}")

    while True:
        data = await reader.read(1024)  # 1024바이트씩 읽는다
        if not data:
            break  # 연결 종료
        await data_received(data,writer)

    print("Connection closed")
    writer.close()
    await writer.wait_closed()

async def data_received(data,writer):
    imgId = int.from_bytes(data[0:4],byteorder="little")
    imgSize = int.from_bytes(data[4:12], byteorder="little")
    imgType = data[12]
    imgData = data[13:]
    imgInfo = {'imgId':imgId, 'imgSize':imgSize, 'imgType':imgType}
    check_imgList(imgInfo)
    imgBinary = make_imgBinary(imgInfo, imgData)
    # 이미지를 모두 수신했으면 빈자리 판단
    if imgBinary:
        print("이미지 모두 전송 받음")
        seatInfo = yolo.judge_vacantSeat(imgBinary)
        print("주차 자리 정보 생성 완료")
        await send_seatInfo(seatInfo,writer)
        print("주차 자리 정보 전송 완료")

def check_imgList(imgInfo):
    # 이미지 리스트를 조회해서 imgId가 없으면 리스트에 추가
    for img in imgList:
        if img['imgId'] == imgInfo['imgId']:
            return
    imgInfo['imgOffset'] = 0
    imgInfo['imgData'] = bytes()
    imgList.append(imgInfo)

def make_imgBinary(imgInfo, imgData):
    for img in imgList:
        if img['imgId'] == imgInfo['imgId']:
            img['imgData'] += imgData
            if len(img['imgData']) == img['imgSize']: # 데이터를 모두 받은 경우
                imgBinary = img['imgData']
                imgList.remove(img)
                return imgBinary
            return None

async def send_seatInfo(seatInfo,writer):
    # json으로 만들어서 클라로 송신
    msgId = 7
    msg = {'msgId': msgId, 'SeatInfo': seatInfo}
    send_data = orjson.dumps(msg)
    writer.write(send_data)
    await writer.drain()

async def main():
    server = await asyncio.start_server(
        handler, '127.0.0.1', 10002)

    async with server:
        await server.serve_forever()


imgList = []
yolo = Car_Detection()
asyncio.run(main())