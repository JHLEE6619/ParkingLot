using System.IO;
using System.Net;
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
    private VideoCapture video_entrance;
    private VideoCapture video_exit;
    private VideoCapture video_parkingLot;
    public Network clnt_entrance;
    public Network clnt_exit;
    public Network clnt_cctv;
    public VM_Main VM_main;
    uint imgId = 0;
    object lockobj = new();

    public MainWindow()
    {
        InitializeComponent();
        VM_main = new();
        clnt_entrance = new(this, VM_main, 10001);
        clnt_exit = new(this, VM_main, 10003);
        clnt_cctv = new(this, VM_main, 10002);
        InitializeVideo();
        DataContext = VM_main;
    }

    private void InitializeVideo()
    {
        string filePath_entrance = @"C:/Users/LMS/Desktop/video/Entrance.mp4";
        string filePath_exit = @"C:/Users/LMS/Desktop/video/Exit.mp4";
        string filePath_parkingLot = @"C:/Users/LMS/Desktop/video/parking_lot.mp4";
        video_entrance = new VideoCapture(filePath_entrance);
        video_exit = new VideoCapture(filePath_exit);
        video_parkingLot = new VideoCapture(filePath_parkingLot);
        Task.Run(() => PlayVideoAsync(video_entrance, Img_Entrance, (byte)Network.MsgId.ENTER_VEHICLE, clnt_entrance));
        Task.Run(() => PlayVideoAsync(video_exit, Img_Exit, (byte)Network.MsgId.EXIT_VEHICLE, clnt_exit));
        Task.Run(() => PlayVideoAsync(video_parkingLot, Img_ParkingLot, (byte)Network.MsgId.SEAT_INFO, clnt_cctv));
    }

    private async Task PlayVideoAsync(VideoCapture video, Image img, byte msgId, Network network)
    {
        Mat matImage = new();
        var captureTimer = new System.Diagnostics.Stopwatch(); // 저장 타이머
        captureTimer.Start();

        while (!video.IsDisposed)
        {
            video.Read(matImage);
            if (matImage.Empty())
                break;

            // 화면에 출력
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
                Send_imgAsync(imgData, msgId, network);                 
                captureTimer.Restart(); // 타이머 리셋
            }
            Thread.Sleep(33);
        }
        video.Release(); // 비디오가 끝나면 닫기
    }

    private async Task Send_imgAsync(byte[] imgData, byte msgId, Network network)
    {
        uint img_id;
        lock (lockobj)
        {
            img_id = imgId++;
        }

        long imgSize = imgData.Length; // 이미지 사이즈
        long remaining_imgSize = imgSize;
        byte[] imgType = [msgId]; // 이미지 타입(0 : 입구 , 1 : 출구, 2 : 주차장)
        int headerSize = sizeof(uint) + sizeof(long) + sizeof(byte);
        byte[] serializedData; // 바이트 배열 컨버트용 버퍼
        byte[] buf = new byte[1024]; // 송신 버퍼
        int offset = 0; // 송신 버퍼용 오프셋
        int readSize = buf.Length - headerSize;
        int readOffset = 0; // 이미지 읽기용 오프셋

        int num = 1;
        while (remaining_imgSize > 0)
        {
            // 이미지 식별자
            serializedData = BitConverter.GetBytes(img_id);
            Array.Copy(serializedData, 0, buf, offset, serializedData.Length);
            offset += sizeof(int);

            // 이미지 크기
            serializedData = BitConverter.GetBytes(imgSize);
            Array.Copy(serializedData, 0, buf, offset, serializedData.Length);
            offset += sizeof(long);

            // 이미지 타입(5 : 입구 , 6 : 출구, 7 : 주차장)
            Array.Copy(imgType, 0, buf, offset, imgType.Length);
            offset += sizeof(byte);
            // 이미지 데이터
            if (remaining_imgSize < readSize) readSize = (int)remaining_imgSize;
            if (imgData == null) break;
            Array.Copy(imgData, readOffset, buf, offset, readSize);
            await network.Stream.WriteAsync(buf, 0, headerSize+readSize).ConfigureAwait(false);

            remaining_imgSize -= readSize;
            readOffset += readSize;
            offset = 0; 
        }

        System.Diagnostics.Debug.WriteLine("이미지 전송");
    }
}