using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace P1_Comm
{
    public class CounterWorker : IDisposable
    {
        public event EventHandler<string> LogOccurred;

        public event EventHandler _TimerSet;
        public event EventHandler WatchdogTimeout;

        private readonly ManualResetEventSlim _stopSignal = new ManualResetEventSlim(false);
        
        private const int TimeoutSecond = 5000;
        private System.Timers.Timer _watchdog;
        private Thread _counterThread;


        private volatile int cnt ;
        private volatile bool _isRunning = false;
        private volatile bool _isPased = false;

        public bool IsRunning => _isRunning;



        public CounterWorker()
        {
            _watchdog = new System.Timers.Timer(TimeoutSecond);
            _watchdog.AutoReset = false;
            _watchdog.Elapsed += OnWatchdogElapsed;
        }

        public void Start()
        {
            if (_isRunning)
            {
                return;
            }

            _isRunning = true;
            _isPased = false;
            _stopSignal.Reset();

            _counterThread = new Thread(CountLoop)
            {
                IsBackground = true,
                Name = "CounterTimer"
            };

            _counterThread.Start();
            _watchdog.Start();
            OnLogOccurred("작업이 시작되었습니다.");

        }

        public void Stop()
        {

            if (!_isRunning)
            {
                return;
            }

            
            
            _isRunning = false;
            _isPased = true;

            _watchdog.Stop();
            _stopSignal.Set();
            if (_counterThread != null && _counterThread.IsAlive)
            {
                _counterThread.Join(200);
                _counterThread = null;
            }
            OnLogOccurred("중단되었습니다 " +
                "");

        }


        private void CountLoop()
        {
            while (_isRunning)
            {
                if (!_isPased)
                {
                    cnt++;
                    OnLogOccurred($"{cnt} 차례 수행");

                    _watchdog.Stop();
                    _watchdog.Start();
                }

                if (_stopSignal.Wait(1000))
                {
                    break;
                }

            }

        }

        public void OnWatchdogElapsed(object sender, ElapsedEventArgs e)
        {
            OnLogOccurred("카운터 신호가 없어서 워치독이만료됨");
            WatchdogTimeout?.Invoke(this, EventArgs.Empty);
        }



        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);


        public void Dispose()
        {
            Stop();

            _watchdog.Elapsed -= OnWatchdogElapsed;
            _watchdog.Dispose();
            _watchdog = null;
            _isPased = false;
            _stopSignal.Dispose();
            cnt = 0;

        }
    }
}
