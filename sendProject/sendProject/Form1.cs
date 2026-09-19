using sendProject.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace sendProject
{
    public partial class Form1 : Form
    {
       
        private Man _man;
        private SerialPort _serial;


        private byte[] _parameter;
        private byte _addr;



        public Form1(Man man)
        {
            InitializeComponent();
            _man = man; // cfg 생성자 주입


            _addr = 0x01;
            _parameter = new byte[500];
            for (int i = 0; i < _parameter.Length; i++)
            {
                _parameter[i] = (byte)(i % 100);
            }

            portList.Items.AddRange(SerialPort.GetPortNames());
            _serial = new SerialPort();
            _serial.BaudRate = 9600;
            _serial.Parity = Parity.None;
            _serial.StopBits = StopBits.One;
            _serial.DataBits = 8;

            _serial.DataReceived += _serial_DataReceived;
            _serial.ReadTimeout = 1000;
            _serial.WriteTimeout = 1000;

            TcpConfig.Instance.OnLogPrint += _model_LogOccurred;

            TcpConfig.Instance.DataReceived += OnTcpDataReceived;

        }
        private void _model_LogOccurred(object sender, string log)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => { txtBox.AppendText($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]: {log}\r\n"); }));
                return;
            }
            if (IsDisposed) { return; }
            
        }
        private void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            // 상대방 COM PORT에서 Send한 메세지를 받는 이벤트
            // 이벤트란?
            // 특정한 조건에서 "비동기"로 동작하는 메소드

            // 문자열은 간단하게 ReadExisting()쓰면 편함!
            // 바이트는 BytesToRead로 갯수 파악후 Read로 받기!
            int length = _serial.BytesToRead;
            if (length < 0) { return; }

            byte[] by = new byte[length];
            byte[] by2 = new byte[length];

            _serial.Read(by, 0, length);
            byte[] data= new byte[length - 3];
            
            Array.Copy(by, 1, data, 0, data.Length);
            String msg = Encoding.UTF8.GetString(data);


            if (InvokeRequired)
            {
                Invoke(new Action(() => { txtBox.Text += $"{Encoding.UTF8.GetString(data)}\n"; }));
            }

            if (msg.Contains("DRS"))
            {
                ProcessDRS(msg);
            }
            else if (msg.Contains("DWR"))
            {
                ProcessDWR(msg);
            }
            else if (msg.Contains("SYNC"))
            {
                ProcessSYNC();
            }
            else if (msg.Contains("DWS"))
            {

            }
            else if (msg.Contains("TEMP"))
            {
                ProcessTemp(msg);
            }
            else if (msg.Contains("CHECK"))
            {
                ProcessCheck(msg);
            }

        }

        private void ProcessDWR(String msg)
        {

            try
            {
                string[] splitData = msg.Split(',');

                if(splitData.Length  < 4) { return ;  }
           

                int cellIdx = int.Parse(splitData[2].Trim());
                int idxValue = Convert.ToInt32(splitData[3], 16);

                if (!_man.isValid(cellIdx, idxValue))
                {
                    Console.WriteLine($"inValid Range Value");
                    return ;
                }
                string data ="";

                _man.SetValue(cellIdx, idxValue);
                _man.Save();
                if (_man.Save() == true)
                {
                    data = $"{_addr:00}DWR,OK";
                }
                else
                {
                    data = $"{_addr:00}DWR,NO";
                }

                byte[] by = new byte[data.Length + 3];


                by[0] = 0x02;
                for (int i = 0; i < data.Length; i++)
                {
                    by[1 + i] = (byte)data[i];
                }
                by[by.Length - 2] = 0x0D;
                by[by.Length - 1] = 0x0A;
                string chk = Encoding.UTF8.GetString(by);
                _serial.Write(by, 0, by.Length);



                if (InvokeRequired)
                {
                    Invoke(new Action(() => { txtBox.Text += $"\n{chk} \n"; }));
                }


            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString()); return ; 
            }

        }

        private void ProcessDRS(String msg)
        {

            Dictionary<int, int> allParams = _man.GetAllParams();

            if (allParams == null || allParams.Count == 0) return;

            int cnt = 1;

            Random rand = new Random();
            String[] splitData = msg.Split(',');
            if (int.TryParse(splitData[1], out int count ))
            {

                if (int.TryParse(splitData[2], out int idx)) {  

                    string data = $"{_addr:00}DRS,OK";
                    for (int i = idx; i < count + idx; i++)
                    {
                        int randomValue = rand.Next(0, 101);

                        if (allParams.ContainsKey(i))
                        {
                            randomValue = TransPvValue(i, allParams[i], randomValue);
                        }
                        data += randomValue.ToString("X4");
                        if (i != idx + count - 1) { data += ","; }
                        cnt++;
                    }

                    byte [] by = new byte[data.Length + 3];
                    by[0] = 0x02; //STX
                    
                    for (int i = 0; i < data.Length; i++)
                    {
                        by[1 + i] = (byte)data[i];
                    }

                    by[by.Length - 2] = 0x0D;
                    by[by.Length - 1] = 0x0A;
                    string chk = Encoding.UTF8.GetString(by);
                    _serial.Write(by, 0 , by.Length);
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => { txtBox.Text += $"send : {chk}\n"; }));
                    }
                }


            }


        }

        private void ProcessSYNC()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { txtBox.Text += $"config : {_man.GetAllParams()} !\r\n"; }));
                }
                Dictionary<int, int> allParams = _man.GetAllParams();

                if (allParams == null || allParams.Count == 0) return;

                StringBuilder sb = new StringBuilder(2048);
                sb.Append($"{_addr:00}SYNC"); // "01SYNC"

                foreach (var item in allParams)
                {
                    int address = item.Key;
                    int val = item.Value;
                    sb.Append($",{address:D4}:{val:X4}");
                }

                string data = sb.ToString();

                byte[] dataBy = Encoding.ASCII.GetBytes(data);
                byte[] sendBy = new byte[dataBy.Length + 3];

                sendBy[0] = 0x02;
                Array.Copy(dataBy, 0, sendBy, 1, dataBy.Length);
                sendBy[sendBy.Length - 2] = 0x0D;
                sendBy[sendBy.Length - 1] = 0x0A;

                _serial.Write(sendBy, 0, sendBy.Length);

                if (InvokeRequired)
                {
                    string logMsg = data.Length > 1000 ? data.Substring(0, 1000) + "" : data;
                    Invoke(new Action(() => { txtBox.Text += $"설정 파일 전송 : {logMsg}\n"; }));
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() => { txtBox.Text += $"설정 파일 전송 에러: {ex.Message}\n"; }));
            }
        }

        private void ProcessTemp(string msg)
        {

            var rnd = new Random();
            //String[] splitData = msg.Split
            string data = $"{_addr:00}TEMP,OK,{ rnd.Next(20, 30)},{ rnd.Next(20, 30)},{ rnd.Next(20, 30)},{ rnd.Next(20, 30)}";

            byte[] body = Encoding.ASCII.GetBytes(data);
            byte[] by = new byte[data.Length + 3];
            by[0] = 0x02; //STX

            Array.Copy(body, 0, by, 1, data.Length);

            by[by.Length - 2] = 0x0D;
            by[by.Length - 1] = 0x0A;
            string chk = Encoding.UTF8.GetString(by);
            _serial.Write(by, 0, by.Length);

            if (InvokeRequired)
            {
                Invoke(new Action(() => { txtBox.Text += $"send : {chk}\n"; }));
            }
        }


        private void ProcessCheck(string msg)
        {
            // camProject SerialComm의 헬스체크("CHECK") 응답 — 형식은 기존 TEMP/DRS와 동일한 STX+payload+CR LF
            string data = $"{_addr:00}CHECK,OK";

            byte[] body = Encoding.ASCII.GetBytes(data);
            byte[] by = new byte[data.Length + 3];
            by[0] = 0x02; //STX

            Array.Copy(body, 0, by, 1, data.Length);

            by[by.Length - 2] = 0x0D;
            by[by.Length - 1] = 0x0A;
            string chk = Encoding.UTF8.GetString(by);
            _serial.Write(by, 0, by.Length);

            if (InvokeRequired)
            {
                Invoke(new Action(() => { txtBox.Text += $"send : {chk}\n"; }));
            }
        }

        private void ProcessTcp(string tcpMsg)
        {
            var t = tcpMsg.Split('/');           // ["T1","바코드","레시피","장비ID","SIDE"]
            if (t.Length < 1) return;

            string type = t[0].Trim();

            if (InvokeRequired)
            {
                Invoke(new Action(() => { txtBox.AppendText($"TCP 데이터 {type}\r\n"); }));
            }

            if (type.Contains("T1")) ProcessT1(t);
            else if (type.Contains("T2")) ProcessT2(t);
            else if (type.Contains("T3")) ProcessT3(t);
            else { 
                if (InvokeRequired)
                {
                    Invoke(new Action(() => { txtBox.AppendText($"TCP 데이터 [수신] 알 수 없는 타입: {tcpMsg}\r\n"); }));
                }   

            }
        }

        private void ProcessT1(string[] t)
        {
            if(t.Length < 5) return; 



            string[] t1 = new string[5];
            bool isValid = true;
            for (int i = 0; i < 5; i++)
            {
                t1[i] = t[i].Trim();
            }
            foreach (var item in t1)
                if (string.IsNullOrEmpty(item)) isValid = false;

            string result = isValid ? "OK" : "NG";
            // 서버 -> 장비 : STX / T1 / 바코드 / 레시피 / 장비ID / SIDE / OK또는NG / (ETX)
            string receivedData = string.Join("/", "T1", t1[1], t1[2], t1[3], t1[4], result);

            SendTcpFramed(receivedData, true);
        }

        private void ProcessT2(string[] t)
        {
            bool isValid = true;

            if (t.Length < 6) isValid = false;
            if (!int.TryParse(t[5], out int t2) || t2 <= 0)
            {
                txtBox.AppendText($"유효하지 않은 Count 값입니다: {t[5]}");
                return;
            }
            string[] t2Val = new string[5];
            string[] t2Sv = new string[t2];
            string[] t2Pv = new string[t2];

            for (int i = 0; i < 5; i++)
            {
                t2Val[i] = t[i].Trim();
            }
            var tVar = t[6].Split(',');


            for (int i = 0; i < t2; i++)
            {
                t2Pv[i] = tVar[i].Trim();
                t2Sv[i] = tVar[i+ t2].Trim();
            }   


            if (InvokeRequired)
            {
                Invoke(new Action(() => { txtBox.AppendText($"수신받인 데이터 ${t}\r\n" +
                    $"T2Pv: {string.Join(",", t2Pv)}\r\nT2Sv: {string.Join(",", t2Sv)}\r\n"); }));
            }
            string result = isValid ? "OK" : "NG";
            string receivedData = string.Join("/", "T2", t2Val[1], t2Val[2], t2Val[3], t2Val[4], t2Sv, t2Pv, result);
            SendTcpFramed(receivedData, isValid);
        }

        private void ProcessT3(string[] t)
        {
            bool isValid = true;

            if (t.Length < 6) isValid = false;

            string[] t3Val = new string[7];

            if (!int.TryParse(t[5], out int t3) || t3 <= 0)
            {
                txtBox.AppendText($"유효하지 않은 Count 값입니다: {t[5]}");
                return;
            }
            for (int i = 0; i < 5; i++)
            {
                t3Val[i] = t[i].Trim();
            }

            var tVar = t[6].Split(',');
            string[] t3Sv = new string[t3];
            string[] t3Pv = new string[t3];

            for (int i = 0; i < t3; i++)
            {
                t3Pv[i] = tVar[i].Trim();
                t3Sv[i] = tVar[i + t3].Trim();
            }


            if (InvokeRequired)
            {
                Invoke(new Action(() => {
                    txtBox.AppendText($"수신받인 데이터 ${t}\r\n" +
                    $"T3Pv: {string.Join(",", t3Pv)}\r\nT3Sv: {string.Join(",", t3Sv)}\r\n");
                }));
            }
            string result = isValid ? "OK" : "NG";
            string receivedData = string.Join("/", "T3", t3Val[1], t3Val[2], t3Val[3], t3Val[4], t3Sv, t3Pv, result);
             SendTcpFramed(receivedData, isValid);
        }

        public void SendTcpFramed (string payload, bool lastVal )
        {
            if (!TcpConfig.Instance.IsServerRunning) return;
            TcpConfig.Instance.SendData(payload, lastVal);
        }

        private int TransPvValue(int cellIdx, int value, int resultValue)
        {
            if (value % 2 == 0)
            {
                return resultValue * 10;
            }
            return resultValue;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void conBtn_Click(object sender, EventArgs e)
        {

            try
            {
                if (_serial.IsOpen)
                {
                    _serial.Close();
                }
                else
                {
                    _serial.PortName = portList.Text;
                    _serial.Open();
                }
                conBtn.Text = _serial.IsOpen ? "DisConnect" : "Connect";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (txtBox.TextLength > 1000)
            {
                txtBox.Clear();
            }
        }

        private void portList_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }

        private void tcpBtn_Click(object sender, EventArgs e)
        {

            if (TcpConfig.Instance.IsServerRunning)
            {
                TcpConfig.Instance.TcpDisconnect();
                
            }
            else
            {
                TcpConfig.Instance.ServerConnect(ipTxt.Text, int.Parse(portTxt.Text) );

            }

            tcpBtn.Text = TcpConfig.Instance.IsServerRunning ? "서버 중지" : "연결";

        }




        private void OnTcpDataReceived(object sender, string data)
        {
            if (InvokeRequired) 
            {
                Invoke(new Action(() => OnTcpDataReceived(sender, data)));
                return;
            }

            if (IsDisposed) return;

            txtBox.AppendText($"[수신] {data}\r\n");
            ProcessTcp(data);
        }



        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            TcpConfig.Instance.DataReceived -= OnTcpDataReceived;   // 누수/예외 방지
            base.OnFormClosed(e);
        }

        private void sendBtn_Click(object sender, EventArgs e)
        {

        }
    }






    public class Param
    {
        public int Name { get; set; } = 0;
        public int Value { get; set; } = 0;

        public Param(int name, int value)
        {
            Name = name;
            Value = value;
        }
    }
}


// portList , conBtn