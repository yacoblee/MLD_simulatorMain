using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class Man
    {
        private SettingCon _cfg;
        public string[] portName = { "" };
        public string[] cmbSpeed = { "4800", "9600", "19200", "38400", "57600", "76800", "115200"};
        public string[] cmbBit = { "None", "Even", "Odd", "Mark", "Space"};
        public string[] cmbLength = { "7", "8", "9"};
        public string[] cmbStopBit = { "One", "Two", "OnePointFirst"};
        public string[] cmbProtocol = { "PC-LINK STD", "PC-LINK+SUM", "MODBUS ASCII", "MODBUS RTU"};
        public string[] cmbDelay = { "" };

        public Man(SettingCon cfg)
        {
            _cfg = cfg;
        }
        public void SetValue(int idx, string value)
        {
            //_cfg.ParamData[idx] = value;
        }


        public bool Save()
        {
            return _cfg.Save();
        }
    }
}
