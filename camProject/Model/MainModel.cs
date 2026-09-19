using camProject.Comm;
using System.IO;
using System.IO.Ports;


namespace camProject.Model
{
    public class MainModel
    {

        public event EventHandler<int[]>? TempReceived;
        public event EventHandler<string>? LogOccurred;
        public event EventHandler? Connected;
        public event EventHandler? Disconnected;

        private System.Timers.Timer? _tempTimer;
        public bool IsOpen => SerialComm.Instance.IsOpen;
        public static string RecipeDir => Path.Combine("C:\\Users\\ss\\Documents\\DevProject\\MLD_simulatorMain\\camProject\\bin\\TempSetting", "Settings");


        public MainModel ()
        {
            SerialComm.Instance.DataReceived += OnData;
            SerialComm.Instance.LogOccurred += (s, m) => LogOccurred?.Invoke(this, m);
            SerialComm.Instance.Connected += (s, e) => Connected?.Invoke(this, e);
            SerialComm.Instance.Disconnected += (s, e) => Disconnected?.Invoke(this, e);
        }

        public void OnData(object sender, string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return;

            string[] tokens = text.Split(',');
            if (tokens.Length < 6) return;

            int [] pv = new int[tokens.Length - 2];

            if (tokens[0].Trim() == "01TEMP" && tokens[1].Trim() == "OK")
            {
                for(int i =0; i< pv.Length; i++)
                {
                    pv[i] = Convert.ToInt32(tokens[i+2].Trim()); 

                }
                TempReceived?.Invoke(this, pv);
            }
        }

        public List<string> GetFileList()
        {
            List<string> result = new List<string>();
            if (!Directory.Exists(RecipeDir)) return result;
            string[] files = Directory.GetFiles(RecipeDir, "*.json");
            for(int i =0; i < files.Length; i++)
            {
                result.Add(Path.GetFileNameWithoutExtension(files[i]));
            }
            
            result.Sort();
            return result;
        }

        public async Task<int[]?> WaitFirstTempAsync(int timeoutMs)
        {
            var tcs = new TaskCompletionSource<int[]>(TaskCreationOptions.RunContinuationsAsynchronously);
            EventHandler<int[]>? handler = null;
            handler = (s, t) => { TempReceived -= handler; tcs.TrySetResult(t); };
            TempReceived += handler;

            SerialComm.Instance.CheckTemp();
            var done = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs));
            TempReceived -= handler;
            return done == tcs.Task ? tcs.Task.Result : null;
        }

        public int[] GetSelectedValue(string fileName) {
            if (string.IsNullOrEmpty(fileName)) return Array.Empty<int>();
            string path = Path.Combine(RecipeDir, fileName + ".json");
            return ConfigFile.Load(path).SelectedValue ?? Array.Empty<int>();
        
        }


        public void SaveSv(int[] sv, string path) => new ConfigFile { SelectedValue = sv }.Save(path);


        public bool Connect(string portName, int baudrate, Parity parity, int dataBits, StopBits stopBits)
            => SerialComm.Instance.Connect(portName, baudrate, parity, dataBits, stopBits);

 
        public void SendFramed(string payload) => SerialComm.Instance.sendFramed(payload);
        public void DisConnect() => SerialComm.Instance.Disconnect();
        public void CheckTemp() => SerialComm.Instance.CheckTemp();


        public void PollingTemp()
        {
            if (_tempTimer == null)
            {
                _tempTimer = new System.Timers.Timer(30000);
                _tempTimer.Elapsed += (s, e) => { if (SerialComm.Instance.IsOpen) SerialComm.Instance.CheckTemp(); };
            }
            _tempTimer.Start();
            if (SerialComm.Instance.IsOpen) CheckTemp();

        }

        public void StopPolling()
        {
            _tempTimer?.Stop();
        }

        public void Dispose()
        {
            StopPolling();
            _tempTimer?.Dispose();
            _tempTimer = null;
        }
    }
}
