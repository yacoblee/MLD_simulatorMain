using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jdh.Data
{
    public class SerialConfig
    {
        public string PortName { get; set; } = "COM1";
        public int BaudRateIndex { get; set; } = 0;
        public int ParityIndex { get; set; } = 0;

        public void Save()
        {
            using (FileStream fs = new FileStream($"{Application.StartupPath}\\SerialSetting.ini", FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    sw.WriteLine($"{PortName}");
                    sw.WriteLine($"{BaudRateIndex}");
                    sw.WriteLine($"{ParityIndex}");
                }
            }
        }
        public static SerialConfig Load()
        {
            SerialConfig config = new SerialConfig();
            FileInfo fi = new FileInfo($"{Application.StartupPath}\\SerialSetting.ini");
            if (!fi.Exists)
            {
                config.Save();
                return config;
            }
            using (FileStream fs = new FileStream($"{Application.StartupPath}\\SerialSetting.ini", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    config.PortName = sr.ReadLine();
                    config.BaudRateIndex = int.Parse(sr.ReadLine());
                    config.ParityIndex = int.Parse(sr.ReadLine());
                }
            }
            return config;
        }
    }
}
