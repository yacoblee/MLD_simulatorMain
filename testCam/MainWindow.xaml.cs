using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using DirectShowLib;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;

namespace testCam;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// 카메라 화면 동작 확인용 테스트 프로젝트 — UI 스레드에서 DispatcherTimer로 프레임을 직접 pull.
/// </summary>
public partial class MainWindow : System.Windows.Window
{
    private VideoCapture? _capture;
    private readonly Mat _frame = new Mat();
    private readonly DispatcherTimer _timer = new DispatcherTimer();

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
        Closed += MainWindow_Closed;
    }

    private const string TargetDeviceName = "openaicam"; // 장치관리자에 표시된 OV2735 장치명(부분 일치)

    private void MainWindow_Loaded(object? sender, RoutedEventArgs e)
    {
        int index = FindDeviceIndexByName(TargetDeviceName);
        if (index < 0)
        {
            statusText.Text = $"'{TargetDeviceName}' 이름의 장치를 DirectShow 목록에서 찾지 못했습니다.";
            MessageBox.Show(statusText.Text);
            return;
        }

        // OpenCV 기본 백엔드(MSMF 등)는 DirectShow와 열거 순서가 다를 수 있어
        // 반드시 DSHOW 백엔드를 명시해야 위에서 찾은 인덱스와 실제 장치가 일치함
        var cap = new VideoCapture(index, VideoCaptureAPIs.DSHOW);
        if (!cap.IsOpened())
        {
            statusText.Text = $"index {index}('{TargetDeviceName}')를 열지 못했습니다.";
            MessageBox.Show(statusText.Text);
            return;
        }

        _capture = cap;
        statusText.Text = $"'{TargetDeviceName}' 연결됨 (DSHOW index {index})";

        _timer.Interval = TimeSpan.FromMilliseconds(33);
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    // DirectShow 비디오 입력 장치 카테고리를 열거해 이름으로 인덱스를 찾는다.
    // OpenCV DSHOW 백엔드가 참조하는 것과 같은 카테고리(KSCATEGORY_VIDEO_INPUT_DEVICE)라 인덱스가 그대로 대응됨.
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
        cameraImage.Source = _frame.ToBitmapSource();
    }

    private void MainWindow_Closed(object? sender, EventArgs e)
    {
        _timer.Stop();
        _capture?.Release();
    }
}
