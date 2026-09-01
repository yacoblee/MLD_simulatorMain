using camProject.Comm;
using camProject.Model;
using OpenCvSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace camProject.ViewModel
{
    // copyApp에는 대응 파일 없음 — MainView.cs의 로직을 UI 비의존 형태로 옮겨받는 새 계층
    //
    // 소유할 것:
    //   - Model.MainModel _mainModel, Model.TcpModel _tcpModel (생성자에서 생성/주입)
    //
    // MainWindow.xaml 컨트롤과 짝지을 바인딩 프로퍼티 (INotifyPropertyChanged 구현 필요):
    //   - Ip, Port, TimeOut, RetryCount, Barcode          ↔ ipTxt / portTxt / timeTxt / retryTxt / barTxt
    //   - RecipeList (ObservableCollection<string>), SelectedRecipe   ↔ registBox
    //   - IsActiveRole (bool)                              ↔ activeRadio / passiveRadio
    //   - Pv1~Pv4 (읽기전용), Sv1~Sv4 (양방향)              ↔ pv1~4Text / sv1~4Box
    //   - LogEntries (ObservableCollection<string>)         ↔ logList
    //
    // 커맨드 (RelayCommand로 구현, MainWindow.xaml.cs의 TODO 핸들러들을 대체):
    //   - TcpConnectCommand, TcpSendCommand, SvSaveCommand
    //   - SerialSaveCommand, SerialConnectCommand, SerialSendCommand
    //
    // 생성자에서 할 일:
    //   - _mainModel.TempReceived += (s, pv) => Pv1~4 갱신 (Dispatcher 마샬링 필요 — 백그라운드 스레드에서 발생)
    //   - _mainModel.LogOccurred / _tcpModel.LogOccurred += (s, msg) => LogEntries에 추가
    //     컬렉션 스레드 동기화는 BindingOperations.EnableCollectionSynchronization 권장
    //     (Dispatcher.Invoke로 매번 감싸는 것보다 이 프로젝트의 로그처럼 자주 갱신되는 컬렉션에 적합)
    //   - _tcpModel.DataReceived 도 동일 패턴으로 LogEntries에 반영
    //
    // 참고 (이 파일이 아니라 MainWindow.xaml.cs / MainWindow.xaml에서 할 일):
    //   - 생성자에서 DataContext = new MainViewModel();
    //   - XAML을 x:Name 기반 코드비하인드 접근 대신 Text="{Binding Ip}" 등 바인딩으로 전환
    //   - 버튼의 Click="..." 대신 Command="{Binding TcpConnectCommand}" 로 교체
    public class MainViewModel : INotifyPropertyChanged
    {
        Model.MainModel _mainModel = new MainModel();
        Model.TcpModel _tcpModel = new TcpModel();
        public ObservableCollection<string> LogEntries { get; set; } = new();
        public ObservableCollection<int> Pv { get; } = new(new[] { 0, 0, 0, 0 });
        public ObservableCollection<int> Sv { get; } = new(new[] { 0, 0, 0, 0 });

        public event PropertyChangedEventHandler? PropertyChanged;


        public MainViewModel()
        {

            _mainModel.TempReceived += (s, pv) =>
                Application.Current.Dispatcher.Invoke(() => {
                    for (int i = 0; i < pv.Length; i++)
                    {
                        Pv[i] = pv[i];
                    }
                });

            SerialComm.Instance.LogOccurred += (s, msg) => LogEntries.Add($"Serial {msg}");
            TcpComm.Instance.LogOccurred += (s, msg) => LogEntries.Add($"TCP {msg}");

            
             

        }


    }
}
