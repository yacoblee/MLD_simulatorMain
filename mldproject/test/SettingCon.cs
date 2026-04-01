using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Newtonsoft.Json;

namespace test
{
    internal class SettingCon
    {
        private readonly static string filePath = Path.Combine(Application.StartupPath, "Settings", "Setting.json");
        public Dictionary<int, string> ParamData { get; set; } = new Dictionary<int, string>();


        public bool Save()
        {
            try
            {
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName)) { Directory.CreateDirectory(directoryName); }
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filePath, json);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"!!!!!설정 파일 저장 실패!!!!!\n상세: {ex.Message}");
                return false;
            }
        }

        public static SettingCon Load()
        {
            if (!File.Exists(filePath))
            {
                var config = new SettingCon();
                config.Save();
                return config;
            }

            string json = File.ReadAllText(filePath);

            var deserializeObj = JsonConvert.DeserializeObject<SettingCon>(json);
            if (deserializeObj == null) { deserializeObj = new SettingCon(); }
            deserializeObj.Save();

            return deserializeObj;
        }



    }



}
}
