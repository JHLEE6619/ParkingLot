using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;
using System.IO;


namespace anprCsharpDotnet1
{
  public class Num_detection
  {

        [DllImport("tsanpr.dll")]
            static extern IntPtr anpr_initialize(
                [MarshalAs(UnmanagedType.LPUTF8Str)] string outputFormat); // [IN] 오류 발생시 출력 데이터 형식
        [DllImport("tsanpr.dll")]
            static extern IntPtr anpr_read_file(
                [MarshalAs(UnmanagedType.LPUTF8Str)] string imgFileName,    // [IN] 입력 이미지 파일명
                [MarshalAs(UnmanagedType.LPUTF8Str)] string outputFormat,   // [IN] 출력 데이터 형식
                [MarshalAs(UnmanagedType.LPUTF8Str)] string options);       // [IN] 기능 옵션
        [DllImport("tsanpr.dll")]
            static extern IntPtr anpr_read_pixels(
                IntPtr pixels,                                              // [IN] 이미지 픽셀 시작 주소
                int width,                                                  // [IN] 이미지 가로 픽셀 수
                int height,                                                 // [IN] 이미지 세로 픽셀 수
                int stride,                                                 // [IN] 이미지 한 라인의 바이트 수
                [MarshalAs(UnmanagedType.LPUTF8Str)] string pixelFormat,    // [IN] 이미지 픽셀 형식
                [MarshalAs(UnmanagedType.LPUTF8Str)] string outputFormat,   // [IN] 출력 데이터 형식
                [MarshalAs(UnmanagedType.LPUTF8Str)] string options);       // [IN] 기능 옵션

        static string ptrToUtf8(IntPtr nativeUtf8)
        {
            if (nativeUtf8 == IntPtr.Zero)
                return "";
            int len = 0;
            while (Marshal.ReadByte(nativeUtf8, len) != 0) ++len;
            byte[] buffer = new byte[len];
            Marshal.Copy(nativeUtf8, buffer, 0, buffer.Length);
            string str = Encoding.UTF8.GetString(buffer);
            buffer = null;
            return str;
        }

        static string readFile(string imgfile, string outputFormat, string options)
        {
            Console.WriteLine("1");
            IntPtr result = anpr_read_file(imgfile, outputFormat, options);
            Console.WriteLine("2");
            return ptrToUtf8(result);
        }

        public static string Execute(string imgPath)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            IntPtr ptr = anpr_initialize("text");
            string error = ptrToUtf8(ptr);
            if (error != "")
            {
                Console.WriteLine(error);
                return null;
            }

            var result = readFile(imgPath, "json", "");
            return Extract_VehicleNum(result);
        }
        
        private static string Extract_VehicleNum(string result)
        {
            string text = "\"text\": ";
            int idx = result.IndexOf(text);
            int num_idx = idx + text.Length + 1;
            string vehicleNum = "";
            for (int i = num_idx; i < result.Length; i++)
            {
                if (result[i] == '"') break;
                vehicleNum += result[i];
            }

            return vehicleNum;
        }
  }
}
