using camProject.Comm;
using camProject.ViewModel;
using OpenCvSharp.Dnn;
using System.IO;
using System.IO.Ports;
using System.Windows;
using System.Windows.Controls;

namespace camProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// UI 이식 단계 — 이벤트 핸들러는 가이드라인 주석만 포함. Model/ViewModel 연결은 다음 단계에서 진행.
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainViewModel ViewModel { get; } = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            DataContext = ViewModel;

            // 시스템 포트 목록만 채움(stdlib 호출) — 나머지 콤보박스 값은 XAML에 고정 값으로 이미 채워둠
            cmbPort.ItemsSource = SerialPort.GetPortNames();
            if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;

        }

        private void CommMode_Changed(object sender, RoutedEventArgs e)
        {
            if (tcpPanel == null || serialPanel == null) return;

            bool isTcp = tcpModeRadio.IsChecked == true;
            tcpPanel.Visibility = isTcp ? Visibility.Visible : Visibility.Collapsed;
            serialPanel.Visibility = isTcp ? Visibility.Collapsed : Visibility.Visible;
        }

        // ===== TCP 패널 =====

        private void BarInputBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 바코드 확정 처리
            // copyApp 대응 없음 — barTxt.Text 확정 시점에 필요하면 여기서 검증(빈 값 등) 후 상태 저장
        }

        // TcpConnBtn_Click 삭제됨 — tcpConnBtn은 이제 Command="{Binding TcpTestCommand}"로 직접 ViewModel 호출 (6단계)

        private void TcpSendBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: T1 시작 전송
            // copyApp 대응: MainView.tcpSend_Click → TcpModel.StartT1Process(barcode, recipe, eqpId, side)
        }

        private void SvSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            Directory.CreateDirectory(Model.MainModel.RecipeDir);
            var dlg = new Microsoft.Win32.SaveFileDialog()
            {
                InitialDirectory = Model.MainModel.RecipeDir,
                Filter = "(*.json)|*.json"
            };
            if (dlg.ShowDialog() != true) return;
            ViewModel.SaveRecipe(dlg.FileName);
        }

        // ===== Serial 패널 =====

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 시리얼 설정 저장
            // copyApp 대응: MainView.saveBtn_Click → MainModel.Config.* 세팅 후 MainModel.Save()
        }

        private void ConnBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ToggleSerialConnect(
                cmbPort.Text,
                int.Parse((cmbSpeed.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "9600"),
                Enum.Parse<Parity>((cmbBit.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "None"),
                int.Parse((cmbLength.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "8"),
                Enum.Parse<StopBits>((cmbStopBit.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "ONE")
            );

        }

        private async void TcpConnBtn_Click(object sender, RoutedEventArgs e)
        {

            bool ok = await ViewModel.TcpConnect(
                    ipTxt.Text,
                    int.Parse(portTxt.Text),
                    int.Parse(timeTxt.Text),
                    int.Parse(retryTxt.Text)
                );

        }
    }
}
