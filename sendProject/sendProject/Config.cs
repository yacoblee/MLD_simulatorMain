using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using Newtonsoft.Json;


namespace sendProject
{

    public class Config
    {
        // 저장 경로
        private readonly static string filePath = Path.Combine(Application.StartupPath, "Settings", "Config.json");
        public Dictionary<int, int> ParamData { get; set; } = new Dictionary<int, int>();

        // 데이터 셀 인덱스
        public int Idx { get; set; } = 0;

        // 설정 값
        public int value { get; set; } = 0;


        public void Save()
        {
            try
            {
                string directoryName = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryName)) { Directory.CreateDirectory(directoryName); }
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
            catch (Exception ex){

                MessageBox.Show($"설정 파일 저장 실패!\n파일이 열려있거나 권한이 없습니다.\n상세: {ex.Message}");

            }
        }

        public static Config Load()
        {
            if (!File.Exists(filePath))
            {
                var config = new Config();
                config.Save();
                return config;
            }

            string json = File.ReadAllText(filePath);

            var deserializeObj = JsonConvert.DeserializeObject<Config>(json);
            if (deserializeObj == null) { deserializeObj = new Config(); }
            deserializeObj.Save();

            return deserializeObj;
        }



    }
}
