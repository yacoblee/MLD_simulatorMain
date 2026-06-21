using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace VersionUp.common
{
    public class ProtocolFile
    {
        private readonly static string filePath = Path.Combine(Application.StartupPath, "Settings", "Setting.json");

        public Dictionary<int, string> ParamData { get; set; } = new Dictionary<int, string>();
        public int SpeedIndex { get; set; } = 1;
        public int BitIndex { get; set; } = 0;
        public int LengthIndex { get; set; } = 1;
        public int StopBitIndex { get; set; } = 0;
        public int ProtocolIndex { get; set; } = 0;


        public Boolean Save()
        {
            try
            {
                string directoryName = Path.GetDirectoryName(filePath) ?? throw new InvalidOperationException("Invalid file path.");
                if (!Directory.Exists(directoryName)) { Directory.CreateDirectory(directoryName); }
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(this, options);
                File.WriteAllText(filePath, json);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 파일 저장 실패: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }

        }

        public static ProtocolFile Load()
        {
            if(!File.Exists(filePath))
            {
                var config = new ProtocolFile();
                config.Save();
                return config;
            }

            try
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<ProtocolFile>(json) ?? new ProtocolFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 파일 로드 실패: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new ProtocolFile();
            }
        }


    }
}
