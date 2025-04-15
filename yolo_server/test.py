import orjson
seatInfo = [[123,24],[3,3,5]]
msg = {'msgId': 7, 'User': None, 'Record': None, 'ParkingList': None, 'SeatInfo': seatInfo}
send_data = orjson.dumps(msg)
print(send_data)