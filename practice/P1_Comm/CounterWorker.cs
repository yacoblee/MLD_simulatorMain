using System;
using System.Threading;
using System.Timers;   // ElapsedEventArgs 용. (Timer 는 모호하므로 아래에서 System.Timers.Timer 로 전체 이름 사용)
// [2026-11-11 수정] 사용하지 않는/IDE 가 자동으로 넣은 using 제거
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace P1_Comm
{
    public class CounterWorker : IDisposable
    {
        public event EventHandler<string> LogOccurred;

        // [2026-11-11 수정] 폼 -> Worker 방향은 이벤트가 아니라 메서드 호출(Start/Stop/Pause/Resume). 사용하지 않아 삭제.
        //public event EventHandler _TimerSet;
        public event EventHandler WatchdogTimeout;

        private readonly ManualResetEventSlim _stopSignal = new ManualResetEventSlim(false);

        private const int TimeoutSecond = 5000;
        private System.Timers.Timer _watchdog;
        private Thread _counterThread;

        private volatile int cnt;
        private volatile bool _isRunning = false;
        // [2026-11-11 수정] 오타 _isPased -> _isPaused. Start/Stop 이 아니라 Pause/Resume 으로만 바꾼다.
        //private volatile bool _isPased = false;
        private volatile bool _isPaused = false;
        private bool _disposed = false;

        public bool IsRunning => _isRunning;
        public bool IsPaused => _isPaused;

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
            _isPaused = false;
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

            // [2026-11-11 수정] 원본: _isRunning=false; _isPased=true; _watchdog.Stop(); _stopSignal.Set(); Join(200)
            //   문제: 워치독을 Join 보다 먼저 멈추면, 워커 스레드가 마지막 tick 에서 워치독을 다시 켜서
            //         정지 5초 뒤에 가짜 경보가 뜰 수 있다. -> 스레드가 끝난 "뒤"에 워치독을 멈춘다.
            _isRunning = false;
            _stopSignal.Set();

            Thread t = _counterThread;
            if (t != null && t.IsAlive && t != Thread.CurrentThread)
            {
                t.Join(500);
            }
            _counterThread = null;

            _watchdog.Stop();
            OnLogOccurred("중단되었습니다.");
            //OnLogOccurred("중단되었습니다 " +
            //    "");
        }

        // [2026-11-11 추가] 워치독 테스트용: 루프는 살아있지만 tick(카운트 + 워치독 리셋)만 멈춘다.
        //   -> 5초 뒤 워치독이 만료되어 경보가 뜬다. Resume 하면 다음 tick 에서 워치독이 다시 시작된다.
        public void Pause()
        {
            if (!_isRunning || _isPaused) return;
            _isPaused = true;
            OnLogOccurred("카운터 일시정지 (워치독 테스트)");
        }

        public void Resume()
        {
            if (!_isRunning || !_isPaused) return;
            _isPaused = false;
            OnLogOccurred("카운터 재개");
        }

        private void CountLoop()
        {
            // [2026-11-11 수정] 일반 Thread 에서 처리되지 않은 예외가 나면 프로세스가 통째로 종료된다.
            //   Dispose 와 겹쳐서 폐기된 객체를 만지는 경우만 조용히 빠져나간다.
            try
            {
                while (_isRunning)
                {
                    if (!_isPaused)
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
            catch (ObjectDisposedException)
            {
            }
        }

        // [2026-11-11 수정] public -> private. 외부에서 부를 이유가 없다.
        //public void OnWatchdogElapsed(object sender, ElapsedEventArgs e)
        private void OnWatchdogElapsed(object sender, ElapsedEventArgs e)
        {
            // Stop 과 겹쳐 만료된 경우 가짜 경보를 막는다. (Elapsed 는 풀 스레드에서 실행된다)
            if (!_isRunning) return;

            OnLogOccurred("카운터 신호가 없어서 워치독이 만료됨");
            WatchdogTimeout?.Invoke(this, EventArgs.Empty);
        }

        private void OnLogOccurred(string message) => LogOccurred?.Invoke(this, message);

        public void Dispose()
        {
            // [2026-11-11 수정] 원본은 _watchdog = null 이라 Dispose 를 두 번 부르면 NullReferenceException,
            //   워커 스레드가 아직 돌고 있으면 null 을 참조했다. null 대입을 하지 않고 중복 호출을 막는다.
            if (_disposed) return;
            _disposed = true;

            Stop();

            _watchdog.Elapsed -= OnWatchdogElapsed;
            _watchdog.Dispose();
            //_watchdog = null;
            _isPaused = false;
            _stopSignal.Dispose();
            cnt = 0;
        }
    }
}
