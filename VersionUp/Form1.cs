using System.IO.Ports;
using VersionUp.common;


namespace VersionUp
{
    public partial class Form1 : Form
    {

        private const byte STX = 0x02;
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;
        private const byte ADDR1 = (byte)'0';
        private const byte ADDR2 = (byte)'1';
        private string _rxBuffer = "";
        public int rgx;

        public Form1(Object cfgFile)
        {
            InitializeComponent();
        }

        private void InitComboBoxes()
        {
            cmbSpeed.DataSource = new BindingSource(new Dictionary<int, string>
        { { 0, "4800" }, { 1, "9600" }, { 2, "19200" }, { 3, "38400" }, { 4, "57600" }, { 5, "76800" }, { 6, "115200" } }, null);
            cmbSpeed.DisplayMember = "Value"; cmbSpeed.ValueMember = "Key";

            cmbBit.DataSource = new BindingSource(new Dictionary<int, string>
        { { 0, "None" }, { 1, "Even" }, { 2, "Odd" }, { 3, "Mark" }, { 4, "Space" } }, null);
            cmbBit.DisplayMember = "Value"; cmbBit.ValueMember = "Key";

            cmbLength.DataSource = new BindingSource(new Dictionary<int, string>
        { { 0, "7" }, { 1, "8" }, { 2, "9" } }, null);
            cmbLength.DisplayMember = "Value"; cmbLength.ValueMember = "Key";

            cmbStopBit.DataSource = new BindingSource(new Dictionary<int, string>
        { { 0, "One" }, { 1, "Two" }, { 2, "OnePointFive" } }, null);
            cmbStopBit.DisplayMember = "Value"; cmbStopBit.ValueMember = "Key";

            cmbProtocol.DataSource = new BindingSource(new Dictionary<int, string>
        { { 0, "PC-LINK STD" }, { 1, "PC-LINK+SUM" }, { 2, "MODBUS ASCII" }, { 3, "MODBUS RTU" } }, null);
            cmbProtocol.DisplayMember = "Value"; cmbProtocol.ValueMember = "Key";

            //cmbNet.DataSource = new BindingSource(new Dictionary<int, string>
            //    { { 0, "NONE" }, { 1, "Demo1" }, { 2, "Demo2" }}, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cmbPortName.Items.AddRange(SerialPort.GetPortNames());
            if (cmbPortName.Items.Count > 0)
            {
                cmbPortName.SelectedIndex = 0;
            }   
            InitComboBoxes();


        }


        private void TextLog_TextChanged(object sender, EventArgs e)
        {
            if(TextLog.Text.Length > 10000)
            {
                TextLog.Clear();
            }
        }








    }
}