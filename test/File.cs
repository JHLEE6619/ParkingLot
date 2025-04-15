using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    class File
    {
        /*static void Main(string[] args)
        {
            Program program = new Program();
            TcpClient clnt = new("127.0.0.1", 10000);
            var stream = clnt.GetStream();

            string imgDir = @"C:\Users\LMS\Pictures\Camera Roll\WIN_20250331_13_13_55_Pro.jpg";
            var file = new FileInfo(imgDir); // 이미지 정보
            int imgId = 0; // 이미지 식별자
            long imgSize = file.Length; // 이미지 사이즈
            byte[] imgType = [2]; // 이미지 타입(0 : 입구 , 1 : 출구, 2 : 주차장)
            int headerSize = sizeof(int) + sizeof(long) + sizeof(byte);
            byte[] serializedInt; // 바이트 배열 컨버트용 버퍼
            byte[] buf = new byte[1024]; // 송신 버퍼
            int offset = 0; // 송신 버퍼용 오프셋
            int readSize = buf.Length - headerSize;
            int readOffset = 0; // 이미지 읽기용 오프셋

            int num = 0;

            while (imgSize > 0)
            {
                // 이미지 식별자
                serializedInt = BitConverter.GetBytes(imgId);
                Array.Copy(serializedInt, 0, buf, offset, serializedInt.Length);
                offset += sizeof(int);

                // 이미지 크기
                serializedInt = BitConverter.GetBytes(imgSize);
                Array.Copy(serializedInt, 0, buf, offset, serializedInt.Length);
                offset += sizeof(long);

                // 이미지 타입(0 : 입구 , 1 : 출구, 2 : 주차장)
                Array.Copy(imgType, 0, buf, offset, imgType.Length);
                offset += imgType.Length;

                // 이미지 데이터
                if (imgSize < readSize) readSize = (int)imgSize;
                byte[]? imgBinary = program.Read_Img(file, readSize, readOffset);
                if (imgBinary == null) break;
                Array.Copy(imgBinary, 0, buf, offset, imgBinary.Length);
                stream.Write(buf, 0, offset + imgBinary.Length); // 정확한 길이만큼 전송

                imgSize -= readSize;
                readOffset += readSize;
                offset = 0;
                Console.WriteLine(++num);
                Console.WriteLine(imgSize);
            }
            Console.ReadLine();

            //DateTime now = DateTime.Now;
            //DateOnly dateOnly = new DateOnly(2025, 04, 03);
            //TimeOnly timeOnly = new TimeOnly(19, 21, 15);
            //DateTime test = new DateTime(dateOnly, timeOnly);
            //TimeSpan dif = test - now;
            //double diff = dif.TotalMinutes;
            //Console.WriteLine(now.ToString("MM월 dd일 HH시 mm분"));
            //Console.WriteLine(test.ToString("MM월 dd일 HH시 mm분"));
            //Console.WriteLine((int)diff);

        }

        private byte[]? Read_Img(FileInfo file, int readSize, int readOffset)
        {
            // 파일이 존재하는지
            if (file.Exists)
            {
                // 바이너리 버퍼
                byte[] binary = new byte[readSize];
                // 파일 IO 생성
                using (var stream = new FileStream(file.FullName, FileMode.Open, FileAccess.Read))
                {
                    // if (readOffset < 0 || readOffset + readSize > stream.Length)
                    //    throw new ArgumentOutOfRangeException(nameof(readOffset), "Offset and size exceed file length");
                    stream.Seek(readOffset, SeekOrigin.Begin); // 파일 처음에서 readOffset만큼 이동
                    // 파일을 IO로 읽어온다.
                    stream.Read(binary, 0, readSize);
                }
                return binary;
            }
            else
                return null;
        }
*/


    }
}
