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
        public MainWindow()
        {
            InitializeComponent();

            // 시스템 포트 목록만 채움(stdlib 호출) — 나머지 콤보박스 값은 XAML에 고정 값으로 이미 채워둠
            cmbPort.ItemsSource = SerialPort.GetPortNames();
            if (cmbPort.Items.Count > 0) cmbPort.SelectedIndex = 0;
        }

        // 상단 Tcp/Seri 라디오 — 패널 표시 전환만 담당 (copyApp에는 없던 로직, WPF 이식 과정에서 추가)
        private void CommMode_Changed(object sender, RoutedEventArgs e)
        {
            if (tcpPanel == null || serialPanel == null) return; // XAML 초기화 시점 이벤트 방어

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

        private void RegistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // TODO: 레시피 선택 시 SV 값 로드
            // copyApp 대응: MainView.registBox_SelectedIndexChanged → MainModel.GetSelectedValue(name) → SetSvRow
            // camProject: CameraModel/향후 이식할 MainModel의 GetSelectedValue 결과를 sv1~4Box에 반영
        }

        private void TcpConnBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: TCP 연결/해제 토글
            // copyApp 대응: MainView.TcpConnBtn_Click → TcpModel.Connect / ServerConn / Disconnect
            // active/passive 라디오(activeRadio/passiveRadio.IsChecked)로 서버·클라이언트 역할 분기 필요
        }

        private void TcpSendBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: T1 시작 전송
            // copyApp 대응: MainView.tcpSend_Click → TcpModel.StartT1Process(barcode, recipe, eqpId, side)
        }

        private void SvSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 레시피 저장 (파일 다이얼로그)
            // copyApp 대응: MainView.regiBtn_Click → SaveFileDialog → MainModel.SaveSv(sv, path)
        }

        // ===== Serial 패널 =====

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 시리얼 설정 저장
            // copyApp 대응: MainView.saveBtn_Click → MainModel.Config.* 세팅 후 MainModel.Save()
        }

        private void ConnBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 시리얼 연결/해제 토글 (+ 최초 연결 시 온도값 확인)
            // copyApp 대응: MainView.connBtn_Click → MainModel.Connect(...) → PreviousConn()에서 WaitFirstTempAsync
        }

        private void SerialSendBtn_Click(object sender, RoutedEventArgs e)
        {
            // TODO: 시리얼 명령 전송
            // copyApp 대응: MainView.serialsnd_Click → MainModel.SendFramed(barcode + ":SWITCH:ON?")
        }
    }
}
