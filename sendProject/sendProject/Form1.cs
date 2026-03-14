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

        private SerialPort _serial;

        private byte[] _parameter;
        private byte _addr;

        public Form1()
        {
            InitializeComponent();
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

            if ( msg.Contains("DRS") ) {
                ProcessDRS(msg);
            }

          
        }

        private void ProcessDRS(String msg)
        {
            Random rand = new Random();
            String[] splitData = msg.Split(',');
            if (int.TryParse(splitData[1], out int count ))  // 반환 해야할 데이터 갯수
            {

                if (int.TryParse(splitData[2], out int idx)) { // regist 역할

                    // return 형식 만들기 
                    string data = $"{_addr:00}DRS,OK";
                    for (int i = idx; i < count + idx; i++)
                    {
                        int randomValue = rand.Next(0, 101);
                        data += randomValue.ToString("X4");
                        if (i != idx + count - 1) { data += ","; }
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
            if (txtBox.TextLength > 10000)
            {
                txtBox.Clear();
            }
        }

        private void portList_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
        }
    }
}


// portList , conBtn