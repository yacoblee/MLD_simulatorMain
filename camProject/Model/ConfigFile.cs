using System;
using System.IO;
using Newtonsoft.Json;

namespace camProject.Model
{
    public class ConfigFile
    {

        public int[] SelectedValue { get; set; } = Array.Empty<int>();

        public bool Save(string path)
        {
            try
            {
                string dir = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                File.WriteAllText(path, json);
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"실패 saving config file: {ex.Message}");
                return false;
            }
        }

        public static ConfigFile Load(string path)
        {
            try {
                if (!File.Exists(path)) return new ConfigFile();
                return JsonConvert.DeserializeObject<ConfigFile>(File.ReadAllText(path)) ?? new ConfigFile();
            }
            catch(Exception ex) { 
                Console.WriteLine("실패 loading config file: " + ex.Message);
                return new ConfigFile();
            }
        }

    }
}
