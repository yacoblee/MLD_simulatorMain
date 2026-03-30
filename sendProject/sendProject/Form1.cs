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
                    conBtn.Text = "Connect";
                }
                else
                {
                    _serial.PortName = portList.Text;
                    conBtn.Text = "DisConnect";
                    _serial.Open();
                }
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