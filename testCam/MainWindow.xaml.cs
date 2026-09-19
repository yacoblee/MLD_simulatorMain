using DirectShowLib;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using testCam.Comm;

namespace testCam;

public partial class MainWindow : System.Windows.Window
{
    private VideoCapture? _capture;
    private readonly Mat _frame = new Mat();
    private readonly DispatcherTimer _timer = new DispatcherTimer();

    private OpenCvSharp.Point? _dragStart;
    private OpenCvSharp.Rect? _roi;

    private readonly TcpServerComm _server = new TcpServerComm();

    public event EventHandler<string>? DataReceived;


    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
        Closed += MainWindow_Closed;
        cameraImage.MouseLeftButtonDown += CameraImage_MouseLeftButtonDown;
        cameraImage.MouseMove += CameraImage_MouseMove;
        cameraImage.MouseLeftButtonUp += CameraImage_MouseLeftButtonUp;
        _server.ClientConnected += TcpClient_staus;
        _server.ClientDisconnected += TcpClient_staus;
        _server.DataReceived += OnDataReceived;

    }

    private void ListenBtn_Click(object sender, RoutedEventArgs e)
    {

        if (_server.IsRunning)
        {
            _server.Stop();
            listenBtn.Content = "서버 시작";
            tcpStateText.Text = "서버 중지됨";
            clientText.Text = "클라이언트: 없음";
        }
        else { 
            string ip = ipTxt.Text.Trim();

            if(string.IsNullOrEmpty(ip) || !int.TryParse(portTxt.Text.Trim(), out int port) || port <= 0 || port > 65535)
            {
                MessageBox.Show("IP 주소와 포트를 올바르게 입력하세요.");
                return;
            }

            bool result = _server.Start(ip,port);

            if(!result)
            {
                MessageBox.Show("서버를 시작하지 못했습니다.");
            } else
            {
                tcpStateText.Text = $"서버 시작됨: {ip}:{port}";
                listenBtn.Content = "서버 중지";
            }

        
        }


    }

    private void TcpClient_staus(object sender, EventArgs e)
    {
        Dispatcher.BeginInvoke(() =>
        {
            if (_server.IsClientConnected)
                clientText.Text = "클라이언트: 연결됨";
            else
                clientText.Text = "클라이언트: 없음";
        });
    }

    public void OnDataReceived(object sender, string text)
    {
        Dispatcher.BeginInvoke(() =>
        {
            string[] tokens = text.Split("/");
            if (tokens.Length < 2) { return; }

            string result = tokens[tokens.Length - 1].Trim();

            if (result == "OK")
            {

            }
            else if (result == "NG")
            {
                SendData("CAM_NG");
            }
            else
            {
                MessageBox.Show($"알 수 없는 응답: {result}");
            }
        });
    }


    public void SendData(string data)
    {
        _server.Send(data);
    }   

    // 화면 상 클릭 좌표(Image 컨트롤 기준) -> 실제 프레임 픽셀 좌표
    // Stretch="Uniform"이라 letterbox 여백이 생길 수 있어 스케일/오프셋 보정 필요
    private OpenCvSharp.Point? ToFramePoint(System.Windows.Point p)
    {
        if (_frame.Empty()) return null;
        double ctrlW = cameraImage.ActualWidth;
        double ctrlH = cameraImage.ActualHeight;
        if (ctrlW <= 0 || ctrlH <= 0) return null;

        double scale = Math.Min(ctrlW / _frame.Width, ctrlH / _frame.Height);
        double offX = (ctrlW - _frame.Width * scale) / 2;
        double offY = (ctrlH - _frame.Height * scale) / 2;

        double x = (p.X - offX) / scale;
        double y = (p.Y - offY) / scale;
        if (x < 0 || y < 0 || x >= _frame.Width || y >= _frame.Height) return null;
        return new OpenCvSharp.Point((int)x, (int)y);
    }

    private static OpenCvSharp.Rect MakeRect(OpenCvSharp.Point a, OpenCvSharp.Point b)
    {
        int x = Math.Min(a.X, b.X);
        int y = Math.Min(a.Y, b.Y);
        int w = Math.Max(1, Math.Abs(a.X - b.X));
        int h = Math.Max(1, Math.Abs(a.Y - b.Y));
        return new OpenCvSharp.Rect(x, y, w, h);
    }

    private static OpenCvSharp.Rect ClampRect(OpenCvSharp.Rect r, OpenCvSharp.Size size)
    {
        int x = Math.Max(0, Math.Min(r.X, size.Width - 1));
        int y = Math.Max(0, Math.Min(r.Y, size.Height - 1));
        int w = Math.Max(1, Math.Min(r.Width, size.Width - x));
        int h = Math.Max(1, Math.Min(r.Height, size.Height - y));
        return new OpenCvSharp.Rect(x, y, w, h);
    }

    private void CameraImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var p = ToFramePoint(e.GetPosition(cameraImage));
        if (p == null) return;
        _dragStart = p;
        cameraImage.CaptureMouse();
    }

    private void CameraImage_MouseMove(object sender, MouseEventArgs e)
    {
        if (_dragStart == null || e.LeftButton != MouseButtonState.Pressed) return;
        var p = ToFramePoint(e.GetPosition(cameraImage)) ?? _dragStart.Value;
        _roi = MakeRect(_dragStart.Value, p);
    }

    private void CameraImage_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        cameraImage.ReleaseMouseCapture();
        _dragStart = null;
    }

    private const string TargetDeviceName = "openaicam";

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        int index = FindDeviceIndexByName(TargetDeviceName);
        if (index < 0)
        {
            statusText.Text = $"'{TargetDeviceName}' 이름의 장치를 DirectShow 목록에서 찾지 못했습니다.";
            MessageBox.Show(statusText.Text);
            return;
        }


        var cap = new VideoCapture(index, VideoCaptureAPIs.DSHOW);
        if (!cap.IsOpened())
        {
            statusText.Text = $"index {index}('{TargetDeviceName}')를 열지 못했습니다.";
            MessageBox.Show(statusText.Text);
            return;
        }
        cap.Set(VideoCaptureProperties.FourCC, VideoWriter.FourCC('M', 'J', 'P', 'G'));
        cap.Set(VideoCaptureProperties.FrameWidth, 1024);
        cap.Set(VideoCaptureProperties.FrameHeight, 768);
        cap.Set(VideoCaptureProperties.Fps, 60);

        _capture = cap;
        statusText.Text = $"'{TargetDeviceName}' 연결됨 (DSHOW index {index})";

        _timer.Interval = TimeSpan.FromMilliseconds(16.67); // 60 FPS
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }


    private static int FindDeviceIndexByName(string nameContains)
    {
        DsDevice[] devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
        for (int i = 0; i < devices.Length; i++)
        {
            if (devices[i].Name?.IndexOf(nameContains, StringComparison.OrdinalIgnoreCase) >= 0)
                return i;
        }
        return -1;
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        if (_capture == null || !_capture.Read(_frame) || _frame.Empty()) return;

        if (_roi != null)
        {
            OpenCvSharp.Rect roi = ClampRect(_roi.Value, _frame.Size());
            Scalar mean = Cv2.Mean(_frame[roi]);
            byte b = (byte)mean.Val0, g = (byte)mean.Val1, r = (byte)mean.Val2;

            colorSwatch.Fill = new SolidColorBrush(System.Windows.Media.Color.FromRgb(r, g, b));
            colorText.Text = $"RGB({r},{g},{b})  #{r:X2}{g:X2}{b:X2}";

            Cv2.Rectangle(_frame, roi, Scalar.Lime, 2);
        }

        cameraImage.Source = _frame.ToBitmapSource();
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _timer.Stop();
        _capture?.Dispose();
        _frame?.Dispose();


        _server.ClientConnected -= TcpClient_staus;
        _server.ClientDisconnected -= TcpClient_staus;
        _server.DataReceived -= OnDataReceived;
        _server.Dispose();
    }
}