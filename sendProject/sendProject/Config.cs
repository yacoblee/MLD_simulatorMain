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
            catch { }
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
