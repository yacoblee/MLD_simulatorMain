using camProject.Comm;
using camProject.Model;
using OpenCvSharp;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO.Ports;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace camProject.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        Model.MainModel _mainModel = new MainModel();
        Model.TcpModel _tcpModel = new TcpModel();

        private readonly object _logLock = new();
        public ObservableCollection<string> LogEntries { get; set; } = new();
        public ObservableCollection<int> Pv { get; } = new(new[] { 0, 0, 0, 0 });
        public ObservableCollection<int> Sv { get; } = new(new[] { 0, 0, 0, 0 });
        public ObservableCollection<string> RecipeList { get; } = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _selectRecipe;

        private string _serialButtonText = "연결";
        private string _tcpButtonText = "연결";

        private void OnPropertyChanged([CallerMemberName] string? name = null) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


        public string SerialButtonText
        {
            get => _serialButtonText;
            set { _serialButtonText = value; OnPropertyChanged(); }
        }

        public string TcpButtonText
        {
            get => _tcpButtonText;
            set { _tcpButtonText = value; OnPropertyChanged(); }
        }

        public string? SelectRecipe
        {
            get => _selectRecipe;
            set
            {
                _selectRecipe = value;
                OnPropertyChanged();
                if (!string.IsNullOrEmpty(value))
                {
                    int[] sv = _mainModel.GetSelectedValue(value);
                    for (int i = 0; i <sv.Length && i < Sv.Count; i++) Sv[i] = sv[i];
                }
            }
        }
        public MainViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(LogEntries, new object());
            _mainModel.TempReceived += (s, pv) =>
                Application.Current.Dispatcher.BeginInvoke(() =>
                {
                    for (int i = 0; i < pv.Length; i++)
                    {
                        Pv[i] = pv[i];
                    }
                });
            _mainModel.Disconnected += (s, e) => Application.Current.Dispatcher.Invoke(() => SerialButtonText = "연결");
            _tcpModel.Connected += (s, e) => Application.Current.Dispatcher.Invoke(() => TcpButtonText = "연결해제");
            _tcpModel.Disconnected += (s, e) => Application.Current.Dispatcher.Invoke(() => TcpButtonText = "연결");

            _mainModel.LogOccurred += (s, msg) => LogEntries.Add($"Serial {msg}");
            _tcpModel.LogOccurred += (s, msg) => LogEntries.Add($"TCP {msg}");

            LoadRecipeList();
            if (RecipeList.Count > 0) SelectRecipe = RecipeList[0];
        }

         public void LoadRecipeList()
        {
            RecipeList.Clear();
            foreach(var item in _mainModel.GetFileList())
            {
                RecipeList.Add(item);
            }
        }

        public void SaveRecipe(string path)
        {
            _mainModel.SaveSv(Sv.ToArray(), path);
            LoadRecipeList();
        }
        public void ToggleSerialConnect(string port, int baud, Parity parity, int dataBits, StopBits stopBits)
        {
            if (_mainModel.IsOpen)
            {
                _mainModel.DisConnect(); 
                return;
            }

            bool ok = _mainModel.Connect(port, baud, parity, dataBits, stopBits);
            if (ok)
            {
                SerialButtonText = "연결해제";
                _mainModel.PollingTemp();
            }
        }

        public async Task<bool> TcpConnect(string ip, int port, int timeOutCnt, int retryCnt)
        {
            if (_tcpModel.IsConnected)
            {
                _tcpModel.DisConnect();
                return true;
            }

            bool ok = await _tcpModel.Connect(ip, port, timeOutCnt, retryCnt);
            if (ok)
            {
                return true;
            } else
            {
                return false;
            }
        }

        public void TcpSend(string barcode)
        {
            if (_tcpModel.IsConnected)
            {
                try
                {
                    int[] pv = Pv.ToArray();
                    int[] sv = Sv.ToArray();
                    string recipe = _selectRecipe ?? "Default";

                    _tcpModel.StartT1Process(barcode, recipe, "TEST1", "Front", pv, sv);
                } catch (Exception ex)
                {
                    MessageBox.Show($"데이터 전송 중 오류 발생: {ex.Message}", "오류");
                }
            }
            else
            {
                MessageBox.Show($"TCP 연결 상태가 아닙니다.");
            }
        }
    }
}
