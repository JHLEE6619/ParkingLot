using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Client1.ViewModel;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace Client1;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : System.Windows.Window
{
    private VideoCapture video_parkingLot;
    private VideoCapture video_entrance;
    private VideoCapture video_exit;
    public Network network;
    uint imgId = 0;
    object lockobj = new();

    public MainWindow()
    {
        //network = new(this);
        InitializeComponent();
        InitializeVideo();
    }

    private void InitializeVideo()
    {
        string filePath_parkingLot = @"video/parking_lot.mp4";
        string filePath_entrance = @"video/Entrance.mp4";
        string filePath_exit = @"video/Exit.mp4";
        video_parkingLot = new VideoCapture(filePath_parkingLot);
        video_entrance = new VideoCapture(filePath_entrance);
        video_exit = new VideoCapture(filePath_exit);
        Task.Run(() => PlayVideo(video_parkingLot, Img_ParkingLot, (byte)Network.MsgId.ENTER_VEHICLE));
        Task.Run(() => PlayVideo(video_entrance, Img_Entrance, (byte)Network.MsgId.ENTER_VEHICLE));
        Task.Run(() => PlayVideo(video_exit, Img_Exit, (byte)Network.MsgId.EXIT_VEHICLE));
    }

    private void PlayVideo(VideoCapture video, Image img, byte msgId)
    {
        Mat matImage = new();
        var captureTimer = new System.Diagnostics.Stopwatch(); // 저장 타이머
        captureTimer.Start();

        while (!video.IsDisposed)
        {
            video.Read(matImage);
            if (matImage.Empty())
                break;

            // 화면에 출력 (ToBitmapSource → Dispatcher)
            var converted = matImage.ToBitmapSource();
            converted.Freeze();
            Dispatcher.Invoke(() =>
            {
                img.Source = converted;
            });

            // 3초마다 이미지 저장
            if (captureTimer.ElapsedMilliseconds >= 3000)
            {
                // 이미지 바이너리 데이터 생성
                Cv2.ImEncode(".jpg", matImage, out byte[] imgData);
                // 서버 전송 메서드
                Send_img(imgData, msgId);                 
                captureTimer.Restart(); // 타이머 리셋
            }
            Thread.Sleep(1);
        }
        video.Release(); // 비디오가 끝나면 닫기
    }

    private void Send_img(byte[] imgData, byte msgId)
    {
        long imgSize = imgData.Length; // 이미지 사이즈
        byte[] imgType = [msgId]; // 이미지 타입(0 : 입구 , 1 : 출구, 2 : 주차장)
        int headerSize = sizeof(uint) + sizeof(long) + sizeof(byte);
        byte[] serializedData; // 바이트 배열 컨버트용 버퍼
        byte[] buf = new byte[1024]; // 송신 버퍼
        int offset = 0; // 송신 버퍼용 오프셋
        int readSize = buf.Length - headerSize;
        int readOffset = 0; // 이미지 읽기용 오프셋

        while (imgSize > 0)
        {
            // 이미지 식별자
            serializedData = BitConverter.GetBytes(imgId);
            Array.Copy(serializedData, 0, buf, offset, serializedData.Length);
            offset += sizeof(int);

            // 이미지 크기
            serializedData = BitConverter.GetBytes(imgSize);
            Array.Copy(serializedData, 0, buf, offset, serializedData.Length);
            offset += sizeof(long);

            // 이미지 타입(0 : 입구 , 1 : 출구, 2 : 주차장)
            Array.Copy(imgType, 0, buf, offset, imgType.Length);
            offset += imgType.Length;

            // 이미지 데이터
            if (imgSize < readSize) readSize = (int)imgSize;
            if (imgData == null) break;
            Array.Copy(imgData, 0, buf, offset, imgData.Length);
            network.Stream.Write(buf, 0, offset + imgData.Length); // 정확한 길이만큼 전송

            imgSize -= readSize;
            readOffset += readSize;
            offset = 0;
        }

        lock (lockobj)
        {
            imgId++;
        }
    }
}