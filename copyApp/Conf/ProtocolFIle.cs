using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copyApp.Conf
{
    public class ProtocolFIle
    {
        public string PortName { get; set; } = "COM1";
        public int BaudRateIndex { get; set; } = 0;
        public int BitIndex { get; set; } = 0;
        public int ParityIndex { get; set; } = 0;
        public int LengthIndex { get; set; } = 0;

        public int StopBitsIndex { get; set; } = 0; 

        public int ProtocolIndex { get; set;} = 0;

        public void Save()
        {
            try
            {
                using (FileStream fs  = new FileStream($"{Application.StartupPath}\\Setting.json", FileMode.Create,FileAccess.ReadWrite ,FileShare.ReadWrite))
                {
                    using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                    {
                        sw.WriteLine($"{PortName}");
                        sw.WriteLine($"{BaudRateIndex}");
                        sw.WriteLine($"{ParityIndex}");
                        sw.WriteLine($"{BitIndex}");
                        sw.WriteLine($"{LengthIndex}");
                        sw.WriteLine($"{StopBitsIndex}");
                        sw.WriteLine($"{ProtocolIndex}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "파일 저장 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }




        public static ProtocolFIle Load()
        {
            ProtocolFIle protocolFIle = new ProtocolFIle();
            FileInfo fi = new FileInfo($"{Application.StartupPath}\\Setting.json");

            if (!fi.Exists){
                    protocolFIle.Save();
                    return protocolFIle;
            }
            using(FileStream fs = new FileStream($"{Application.StartupPath}\\Setting.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) 
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
                    // 줄이 없으면(구버전 파일) 기본값 0으로 안전 처리
                    int ReadInt() => int.TryParse(sr.ReadLine(), out int v) ? v : 0;

                    protocolFIle.PortName = sr.ReadLine() ?? "COM8";
                    protocolFIle.BaudRateIndex = ReadInt();
                    protocolFIle.ParityIndex = ReadInt();
                    protocolFIle.BitIndex = ReadInt();
                    protocolFIle.LengthIndex = ReadInt();
                    protocolFIle.StopBitsIndex = ReadInt();
                    protocolFIle.ProtocolIndex = ReadInt();
                }
            }

            return protocolFIle;

        }

    }

}
