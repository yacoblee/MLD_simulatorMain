using camProject.Comm;
using System.IO.Ports;

namespace camProject.Model
{
    public class TcpModel : IDisposable
    {
        public event EventHandler<string> DataReceived;
        public event EventHandler<string> LogOccurred;
        public event EventHandler<string> ProcessCompleted;

        public event EventHandler Connected; 
        public event EventHandler Disconnected;

        private System.Timers.Timer _retryTimer;
        private int _maxRetryCount = 0;
        private int _currentRetryCount = 0;

        private int[] _sv;
        private int[] _pv;
        public void UpdatePv(int[] pv) => _pv = pv;
        public void UpdateSv(int[] sv) => _sv = sv;
        public bool IsConnected => TcpComm.Instance.IsConnected;

        private enum ProcessState
        {
            Idle,
            WaitingT1Response,
            WaitingT2Response,
            WaitingT3Response
        }
        private ProcessState _currentState = ProcessState.Idle;

        private string _barcode;
        private string _recipe;
        private string _eqpId;
        private string _side;
        private const string DELIM = "/";

        public TcpModel()
        {
            TcpComm.Instance.DataReceived += (s, msg) => DataReceived?.Invoke(this, msg);
            TcpComm.Instance.LogOccurred += (s, m) => LogOccurred?.Invoke(this, m);
            TcpComm.Instance.Connected += (s, e) => Connected?.Invoke(this, e);
            TcpComm.Instance.DisconnectedEvent += (s, e) => Disconnected?.Invoke(this, e);
        }


        public Task <bool> Connect(string ip, int port, int timeOutCnt, int retryCnt)
            => TcpComm.Instance.Connect( ip, port, timeOutCnt, retryCnt);

        public void DisConnect() => TcpComm.Instance.Disconnect();


        public void Dispose  ()
        {
            TcpComm.Instance.Disconnect();
        }
    }
}
