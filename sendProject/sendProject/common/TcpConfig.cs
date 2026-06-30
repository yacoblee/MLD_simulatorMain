using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace sendProject.common
{
    public class TcpConfig : IDisposable
    {

        public static TcpConfig Instance { get; } = new TcpConfig();
        private TcpClient _client;
        private TcpListener _listener;

        private volatile bool _serverRunning;
        public event EventHandler Connected;
        public event EventHandler DisconnectedEvent;
        public event EventHandler<string> OnLogPrint;

        private NetworkStream _stream;

        private Thread _rxThread;
        private Thread _serverThread;

        public bool IsServerRunning => _serverRunning;


        public TcpConfig() { }



        public bool ServerConnect (string host, int port)
        {
            try {
                IPAddress ipAddress = host == "any" ? IPAddress.Any : IPAddress.Parse(host);

                _listener = new TcpListener(ipAddress, port);
                _listener.Start();

                _serverRunning = true;


                _serverThread = new Thread(ServerAccept) { IsBackground = true, Name ="Server Tx-Rx" };

                _serverThread.Start();
                LogPrint($"server start {ipAddress} : {port} \r time: {DateTime.Now: yyyy-mm-dd:HH:m:s}");

                return true;
            } catch (Exception ex){
 
                LogPrint($"서버 시작 실패: {ex.Message.ToString()}");
                return false;
            }
        }


        public void TcpDisconnect()
        {
            if (_listener != null)
            {
                _serverRunning = false;
                _listener.Stop();
                _serverThread?.Join();
                _listener = null;
            }
        }

        private void ServerAccept()
        {
            try
            {
                while (_serverRunning)
                {
                    TcpClient tcpClient = _listener.AcceptTcpClient();
                    _client = tcpClient;
                    _stream = tcpClient.GetStream();

                    _serverRunning = true;




                    Connected?.Invoke(this, EventArgs.Empty);
                    LogPrint($"client connected: {tcpClient.Client.RemoteEndPoint}");

                }
            }catch { 
            
            
            }
        }

 


        private void LogPrint(string msg) => OnLogPrint?.Invoke(this, msg);

        public void Dispose()
        {
            _serverRunning = false;
            try { _listener?.Stop(); } catch { }
            try { _stream?.Close(); } catch { }
            try { _client?.Close(); } catch { }
            _serverThread?.Join(300);       
            _listener = null;
            _serverThread = null;
        }
    }
}
