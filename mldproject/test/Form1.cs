using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace test
{
    public partial class Form1 : Form
    {

        private const byte STX = 0x02;
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;
        private const byte ADDR1 = (byte)'0';
        private const byte ADDR2 = (byte)'1';

        private Dictionary<int, Param> dic; 

        private List<ReceivedData> receiveData = new List<ReceivedData>(); // DRS 결과값 저장할 변수

        private List<ReceivedData> timerData = new List<ReceivedData>(); // Timer 로 받아오는 데이터

        private object receiveLock = new object(); 

        private Form2 _frm2;
        private SerialPort _serial;
        private System.Timers.Timer _timer;
        
        
        public int rgx ;


        List<DrsRequest> request1 = new List<DrsRequest>{
                    new DrsRequest(1, 32), new DrsRequest(33, 32), new DrsRequest(65, 32),
                    new DrsRequest(97, 3), new DrsRequest(101, 32), new DrsRequest(133, 32),
                    new DrsRequest(165, 32), new DrsRequest(201, 32), new DrsRequest(233, 32),
                    new DrsRequest(265, 32), new DrsRequest(301, 32), new DrsRequest(333, 32),
                    new DrsRequest(365, 32), new DrsRequest(401, 32), new DrsRequest(433, 32),
                    new DrsRequest(465, 32)
                };

        List<DrsRequest> request2 = new List<DrsRequest> { new DrsRequest(1, 32) };






        public Form1()
        {
            InitializeComponent();

            _serial = new SerialPort();
            InitChart();

            _timer = new System.Timers.Timer();
            _timer.Interval = 5000;
            _timer.Elapsed += _timer_Elapsed;

            //_serial.PortName = "COM4"; // 메인pc 역활
            //_serial.BaudRate = 9600;
            //_serial.Parity = Parity.None;
            //_serial.DataBits = 8;
            //_serial.ReadTimeout = 1000;
            //_serial.WriteTimeout = 1000;
            _serial.DataReceived += _serial_DataReceived;
            //_serial.Open();

            //_timer.Tick += _timer_Tick;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }



        private void _serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            //마샬링 -> 비동기 ( UI 제어 불가 ) -> 제어문 안으로 진입 - 화면이 안멈춤
            //false 는 동기 실행으로 UI 제어 가능 - 동기로 인한 화면 멈춤
            //if (InvokeRequired)
            //{   
            //    // 동기 상태로 UI 제어 하겠다.
            //    Invoke(new Action(() => { _serial_DataReceived(sender, e); } ));
            //    return;
            //}
            // 상대방 COM PORT 에서 Send 한 메시지를 받는 이벤트 
            // 이벤트 = 비동기로 동작(특정한 조건)
            //string data = _serial.ReadLine();
            // Byte 받아서 읽을 떄는 ByteToRead 로 갯수 파악해서 read
            //string data = _serial.ReadExisting();
            //if (data == null) { return; }
            //TxtLog.Text += data;
            /*         int length = _serial.BytesToRead;

                     byte[] byt = new byte[length];
                     _serial.Read(byt, 0, byt.Length);

                     byte[] data = new byte[byt.Length - 3];
                     Array.Copy(byt, 1, data, 0, data.Length);

                     String msg = Encoding.UTF8.GetString(data);
         */
            
            if (IsDisposed) // 갱신할 대상의 존재 유무 파악 용도
            {
                return;
            }

            int len = _serial.BytesToRead;
            if (len > 0)
            {
                byte[] ret = new byte[len];
                _serial.Read(ret, 0, ret.Length);

                string text = Encoding.ASCII.GetString(ret);
                //TxtLog.Text += $"[수신] {text}\r\n";
                Thread.Sleep(1000);
                inputToken(rgx, text);


            }
          
        }






        private void Buttonwho_Click(object sender, EventArgs e)
        {

            if (!_serial.IsOpen) { return; }


            try
            {

                List<byte> list = new List<byte>();
                list.Add(STX);
                list.Add(ADDR1);
                list.Add(ADDR2);

                list.Add((byte)'W');
                list.Add((byte)'H');
                list.Add((byte)'O');

                list.Add(CR);
                list.Add(LF);
                byte[] cmd = list.ToArray();

                TxtLog.Text += "\r\n WHO click! \r\n";
                _serial.Write(cmd, 0, cmd.Length);

                Thread.Sleep(200);

                int len = _serial.BytesToRead;
                if (len > 0)
                {
                    byte[] ret = new byte[len];

                    _serial.Read(ret, 0, ret.Length);
                    TxtLog.Text += $"{Encoding.UTF8.GetString(ret)} \r\n";
                }
                else
                {
                    TxtLog.Text += "none \r\n";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbPortName.SelectedIndex < 0)
                {
                    return;
                }

                if (!_serial.IsOpen)
                {
                    _serial.PortName = cmbPortName.Text;
                    _serial.BaudRate = 9600;
                    _serial.DataBits = 8;
                    _serial.Parity = Parity.Even;
                    _serial.StopBits = StopBits.One;
                    _serial.Open();
                    buttonConnect.Text = "Disconnect";
                }
                else
                {
                    _serial.Close();
                    buttonConnect.Text = "Connect";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cmbPortName_DropDown(object sender, EventArgs e)
        {
            cmbPortName.Items.Clear();
            cmbPortName.Items.AddRange(SerialPort.GetPortNames());
        }

        private void TxtLog_TextChanged(object sender, EventArgs e)
        {
            if (TxtLog.Text.Length > 10000)
            {
                TxtLog.Clear();
            }

        }



        public async Task<List<ReceivedData>> ExecuteDrsCommunication()
        {
            if (!_serial.IsOpen) return null;

            receiveData.Clear();

            try
            {

                foreach (DrsRequest req in request1)
                {
                    SendAndReceiveDRS(req.Address, req.Count);
                    await Task.Delay(400);
                }

                return receiveData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"통신 중 에러 발생: {ex.Message}");
                return null;
            }
        }


        public List<ReceivedData> ExecuteDrsTimer()
        {
            if (!_serial.IsOpen) return null;
        
            receiveData.Clear();

            try
            {
                
                foreach (DrsRequest req in request2)
                {
                    rgx = req.Address;
                    int firstCount = receiveData.Count;
                    int fullCount = firstCount + req.Count;



                    SendAndReceiveDRS(req.Address, req.Count);

                    int first = 0;
                    while (first < fullCount)
                    {
                        first++;
                        
                        if (first > 50)
                        {
                            break;
                        }
                    }
                }

                return receiveData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"통신 중 에러 발생: {ex.Message}");
                return null;
            }
        }


        private void SendAndReceiveDRS(int startAddr, int count)
        {
            try
            {
                string body = string.Format("DRS,{0:D2},{1:D4}", count, startAddr);

                List<byte> list = new List<byte>();
                list.Add(STX);
                list.Add(ADDR1);
                list.Add(ADDR2);


                list.AddRange(Encoding.ASCII.GetBytes(body));

                list.Add(CR);
                list.Add(LF);

                byte[] cmd = list.ToArray();

                _serial.Write(cmd, 0, cmd.Length);

                Task.Delay(300);
                rgx = startAddr;
                //int len = _serial.BytesToRead;
                //if (len > 0)
                //{
                //    byte[] ret = new byte[len];
                //    _serial.Read(ret, 0, ret.Length);

                //    string text = Encoding.ASCII.GetString(ret);
                //    //TxtLog.Text += $"[수신] {text}\r\n";

                //    inputToken(startAddr, text);
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            List<ReceivedData> result =  ExecuteDrsTimer();

            if (result != null)
            {
                UpdateChart(result);
            }
        }

        private void UpdateChart(List<ReceivedData> dataList)
        {
            try
            {
                if (IsDisposed)
                {
                    return; 
                }

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateChart(dataList)));
                    return;
                }
              
                if (dataList == null || dataList.Count < 4) return;

                DateTime now = DateTime.Now;       
                for (int i = 0; i < 4; i++)
                {
                    string seriesName = $"PV{i + 1}";


                    chart1.Series[seriesName].Points.AddXY(now, dataList[i].data);

                    if (chart1.Series[seriesName].Points.Count > 50)
                    {
                        chart1.Series[seriesName].Points.RemoveAt(0);
                    }

                    var area = chart1.ChartAreas[$"ChartArea{i + 1}"];
                    area.AxisX.Minimum = chart1.Series[seriesName].Points[0].XValue;
                    area.AxisX.Maximum = now.ToOADate();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }
        public void inputToken(int startAddr, string text)
        {
            string cleanText = text.Replace("\r", "").Replace("\n", "");
            string[] tokens = cleanText.Split(',');
            for (int i = 2; i < tokens.Length; i++)
            {
                string hexString = tokens[i].Trim();
                int decimalValue = Convert.ToInt16(hexString, 16);

                receiveData.Add(new ReceivedData(startAddr + (i - 2), decimalValue));
            }
            rgx = 0;
        }

        public class DrsDataEventArgs : EventArgs
        {
            public List<ReceivedData> DataList { get; set; }
            public DrsDataEventArgs(List<ReceivedData> data) => DataList = data;
        }

        private void Popup_Click(object sender, EventArgs e)
        {
            Form2 popup = new Form2();

            popup.StartPosition = FormStartPosition.CenterParent;
            popup.DrsRequestTriggered += async (s, args) =>
            {
                var resultList = await ExecuteDrsCommunication();

                if (resultList != null)
                {
                    popup.DataReceived(this, new DrsDataEventArgs(resultList));
                }
            };
            popup.ShowDialog();
        }
        private void InitChart()
        {
            chart1.Series.Clear();
            //chart1.ChartAreas.Char = SeriesChartType.Line;

            for (int i = 1; i <= 4; i++)
            {
                ChartArea area = chart1.ChartAreas[$"ChartArea{i}"];

                area.AxisY.Minimum = 0;
                area.AxisY.Maximum = 100;
                area.AxisY.Interval = 10;

                area.AxisY.LabelStyle.Format = "0000.00";


                area.AxisX.Minimum = DateTime.Now.ToOADate();
                area.AxisX.Maximum = DateTime.Now.AddMinutes(5).ToOADate();
                area.AxisX.LabelStyle.Format = "MM-dd HH:mm";

                area.AxisY.MajorGrid.LineColor = Color.LightGray;
                area.AxisX.MajorGrid.LineColor = Color.LightGray;

                Series series = new Series($"PV{i}");
                series.ChartArea = $"ChartArea{i}";
                series.ChartType = SeriesChartType.Line;   
                series.XValueType = ChartValueType.DateTime; 
                series.BorderWidth = 1;                    
        
                chart1.Series.Add(series);
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {
 
        }

        private void btTimer_Click(object sender, EventArgs e)
        {

            if (!_timer.Enabled)
            {
                btTimer.Text = "Stop";
                List<ReceivedData> initialData = ExecuteDrsTimer();
                UpdateChart(initialData);
                

                _timer.Start();
            }
            else
            {
                btTimer.Text = "Start";
                _timer.Stop();
            }
        }

        private void cmbPortName_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            
        }
    }
    public class Regist
    {
        public string rgx { get; set; } = "";
        
        public Regist(string rgx) {
        this.rgx = rgx;   
        }
    }
    public class Param
    {
        public string Name { get; set; } = "";
        public double Value { get; set; } = 0.0d;
        public Param(string name, double value)
        {
            Name = name;
            Value = value;
        }

    }
    public class ReceivedData
    {
        public int idx { get; set; }
        public int data { get; set; }
        public ReceivedData(int idx, int data)
        {
            this.idx = idx;
            this.data = data;
        }
    }

    public struct DrsRequest
    {
        public int Address;
        public int Count;

        public DrsRequest(int addr, int cnt)
        {
            this.Address = addr;
            this.Count = cnt;
        }
    }
}
