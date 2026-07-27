using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace copyApp.Comm
{
    // 레시피 1개 = SV 값 배열. 파일 하나당 이 객체 하나를 json으로 저장.
    public class ConfigFile
    {
        public int[] SelectedValue { get; set; } = Array.Empty<int>();

        // 지정 경로에 저장
        public bool Save(string path)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"!!!!!레시피 저장 실패!!!!!\n상세: {ex.Message}");
                return false;
            }
        }

        // 지정 경로에서 로드
        public static ConfigFile Load(string path)
        {
            try
            {
                if (!File.Exists(path)) return new ConfigFile();
                return JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(path)) ?? new ConfigFile();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"!!!!!레시피 로드 실패!!!!!\n상세: {ex.Message}");
                return new ConfigFile();
            }
        }
    }
}
