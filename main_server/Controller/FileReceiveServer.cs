using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Model;
using System.IO;
using anprCsharpDotnet1;

namespace Server.Controller
{
    public class FileReceiveServer : Date
    {
        public Dictionary<int, int> ImgOffset { get; set; } = [];
        public DBC Dbc { get; set; }
        readonly object lock1 = new();

        public FileReceiveServer(int port)
        {
            StartFileRcvServer(port);
        }

        enum MsgId
        {
            ENTRY_RECORD = 0, PAYMENT = 1, INIT_PARKING_LIST = 4, ENTER_VEHICLE = 5, EXIT_VEHICLE = 6,
        }
        enum Vehicle_class
        {
            Normal, PrePayment, Registration
        }

        public async Task StartFileRcvServer(int port)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, port);
            this.Dbc = new();
            Console.WriteLine(" 파일 전송서버 시작 ");
            listener.Start();
            while (true)
            {
                TcpClient client =
                    await listener.AcceptTcpClientAsync().ConfigureAwait(false);

                Task.Run(() => ServerMain(client, port));
            }
        }

        // 클라이언트 별로 다른 스레드가 실행하는 메소드
        private async Task ServerMain(Object client, int port)
        {
            TcpClient tc = (TcpClient)client;
            NetworkStream stream = tc.GetStream(); // Client1, Client4가 연결됨
            Console.WriteLine(" 클라이언트 연결됨 ");
            // Client1이 연결되면 주차리스트 보내기
            Save_clnt(port, stream);
            if (port == 10001)
            {
                await Send_messageAsync(Send_all_record(), stream);
                Console.WriteLine("주차 기록 초기화");
            }

            // 헤더 : 이미지식별자(int), 이미지 크기(int), 이미지 유형(byte) 
            // 바디 : 이미지 binary 데이터
            byte[] buf = new byte[1024];
            int imgId;
            long imgSize;
            byte imgType;
            int headerSize = sizeof(int) + sizeof(long) + sizeof(byte);
            byte[] imgBinary;
            int offset = 0;
            while (true)
            {
                int received = 0, len;
                len = await stream.ReadAsync(buf, 0, buf.Length).ConfigureAwait(false);
                if (len <= 0)
                {
                    Console.WriteLine("클라이언트 연결 종료");
                    break; // 연결 종료
                }

                imgId = BitConverter.ToInt32(buf.AsSpan()[0..4]);
                imgSize = BitConverter.ToInt64(buf.AsSpan()[4..12]);
                imgType = buf[12];

                imgBinary = buf[13..len];
                ImgOffset.TryAdd(imgId, offset); // TryAdd 키가 없으면 추가하고 true 반환, 있으면 추가하지 않고 false 반환

                int writeSize;
                if (imgSize < imgBinary.Length)
                    writeSize = (int)imgSize;
                else writeSize = imgBinary.Length;

                string imgPath = Write_img(imgBinary, imgId, imgType, ImgOffset[imgId], writeSize);
                ImgOffset[imgId] += imgBinary.Length; // 실제로 읽은만큼 offset
                if (imgSize == ImgOffset[imgId])
                {
                    Console.WriteLine($"이미지 타입 : {imgType}\n 이미지 ID : {imgId}");
                    Console.WriteLine("이미지 저장 완료");
                    Handler(imgType, imgPath, stream);
                }
            }
        }

        private void Save_clnt(int port, NetworkStream stream)
        {
            if (port == 10001)
            {
                lock (lock1)
                {
                    Program.Clients[1] = stream;
                }
            }
            else if (port == 10003)
            {
                lock (lock1)
                {
                    Program.Clients[2] = stream;
                }
            }
            else if (port == 10004)
            {
                lock (lock1)
                {
                    Program.Clients[4] = stream;
                }
            }
        }

        private string Write_img(byte[] imgBinary, int imgId, byte imgType, int offset, int writeSize)
        {
            string folderName = "";
            switch (imgType)
            {
                case (byte)MsgId.ENTER_VEHICLE: folderName = "Entrance"; break;
                case (byte)MsgId.EXIT_VEHICLE: folderName = "Exit"; break;
            }
            string path = @$"/img/{folderName}";

            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
                Console.WriteLine("폴더 생성");
            }

            string saveDir = path + @$"/{imgId}.jpg";
            using (var stream = new FileStream(saveDir, FileMode.OpenOrCreate, FileAccess.Write))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                // 파일 쓰기
                stream.Write(imgBinary, 0, writeSize);
            }
            return saveDir;
        }

        private void Handler(byte imgType, string imgPath, NetworkStream stream)
        {
            string vehicleNum;
            lock (lock1)
            {
                vehicleNum = Num_detection.Execute(imgPath);
            }
            if (vehicleNum.Equals("")) return;
            Send_msg send_msg;
            // 입차
            if (imgType == (byte)MsgId.ENTER_VEHICLE)
            {
                send_msg = Show_entryRecord(vehicleNum); // 입차 시 처리 메서드
                Send_messageAsync(send_msg, Program.Clients[1]);
                Console.WriteLine("주차 기록 전송 완료");
            }
            // 출차
            else if (imgType == (byte)MsgId.EXIT_VEHICLE)
            {
                Send_msg exit_msg = exit_vehicleNum(vehicleNum);
                Send_messageAsync(exit_msg, Program.Clients[1]);
                send_msg = Show_paymentInfo(vehicleNum);
                if (send_msg != null)
                {
                    //Send_messageAsync(send_msg, Program.Clients[4]); // 출차 차량 정보를 화면에 띄운다 -> 사전정산/정기등록 차량이면 결제화면 패스
                }
            }
        }

        private Send_msg exit_vehicleNum(string vehicleNum)
        {
            Send_msg msg = new();
            msg.MsgId = (byte)MsgId.EXIT_VEHICLE;
            msg.Record = new()
            {
                VehicleNum = vehicleNum
            };
            return msg;
        }


        private Send_msg Show_entryRecord(string vehicleNum)
        {
            byte cls = Dbc.Select_expDate(vehicleNum);
            Dbc.Insert_Entry_record(cls, vehicleNum);
            Console.WriteLine("주차기록 삽입 완료");
            Entry_exit_record record = Dbc.Select_record(vehicleNum);
            string entry_date = Date_to_str(record.EntryDate);
            Send_msg send_msg = new()
            {
                MsgId = (byte)MsgId.ENTRY_RECORD,
                Record = new()
                {
                    VehicleNum = record.VehicleNum,
                    EntryDate = entry_date,
                    Classification = record.Classification
                }
            };

            return send_msg;
        }

        private Send_msg Show_paymentInfo(string vehicleNum)
        {
            Entry_exit_record record = Dbc.Select_record(vehicleNum);

            string entry_date = Date_to_str(record.EntryDate);
            DateTime now = DateTime.Now;
            string exit_Date = Date_to_str(now);
            int parkingTime = Dif_date(record.EntryDate, now);
            string parking_time = parkingTime + "분";
            int totalFee = Cal_totalFee(parkingTime);
            if (totalFee <= 0) return null;
            string total_fee = totalFee.ToString() + "원";
            // 같은 시점에서 업데이트하면 안되고, 결제하는 시점으로 바뀌어야함
            Dbc.Update_exitRecord(vehicleNum, now, totalFee);
            Send_msg send_msg = new()
            {
                MsgId = (byte)MsgId.PAYMENT,
                Record = new()
                {
                    VehicleNum = record.VehicleNum,
                    EntryDate = entry_date,
                    Classification = record.Classification,
                    ExitDate = exit_Date,
                    ParkingTime = parking_time,
                    TotalFee = total_fee
                }
            };

            return send_msg;
        }

        private Send_msg Send_all_record()
        {
            List<Entry_exit_record> recordList = Dbc.Select_all_record();
            List<Record> parkingList = [];
            foreach (var record in recordList)
            {
                Record park = new()
                {
                    VehicleNum = record.VehicleNum,
                    EntryDate = Date_to_str(record.EntryDate),
                    Classification = record.Classification
                };
                parkingList.Add(park);
            }

            Send_msg send_msg = new()
            {
                MsgId = (byte)MsgId.INIT_PARKING_LIST,
                ParkingList = parkingList
            };

            return send_msg;
        }

        private async Task Send_messageAsync(Send_msg msg, NetworkStream stream)
        {
            string json = JsonConvert.SerializeObject(msg);
            byte[] buf = Encoding.UTF8.GetBytes(json);
            await stream.WriteAsync(buf).ConfigureAwait(false);
        }
    }
}
