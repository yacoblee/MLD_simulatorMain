using System.IO;
using System.Text;

namespace camProject.Model
{
    public class ProtocolFile
    {
        public string PortName { get; set; } = "COM1";
        public int BaudRateIndex { get; set; } = 0;
        public int BitIndex { get; set; } = 0;
        public int ParityIndex { get; set; } = 0;
        public int LengthIndex { get; set; } = 0;
        public int StopBitsIndex { get; set; } = 0;
        public int ProtocolIndex { get; set; } = 0;


        public void Save()
        {
            try
            {
                using (FileStream fs = new FileStream($"{AppContext.BaseDirectory}\\ProtocolSetting.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                Console.WriteLine(ex.ToString());
            }
        }



        public static ProtocolFile Load()
        {

            ProtocolFile protocolFIle = new ProtocolFile();

            FileInfo fileInfo = new FileInfo($"{AppContext.BaseDirectory}\\ProtocolSetting.json");
            if (!fileInfo.Exists)
            {
                protocolFIle.Save();
                return protocolFIle;
            }
            using (FileStream fs = new FileStream($"{AppContext.BaseDirectory}\\ProtocolSetting.json", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
                {
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
