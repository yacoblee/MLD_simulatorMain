using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test
{
    public class Man
    {
        public SettingCon Config => _cfg;
        private SettingCon _cfg;

        public Man(SettingCon cfg)
        {
            _cfg = SettingCon.Load();
        }
        public void SetValue(int idx, string value)
        {
            
        }
   

        public bool Save()
        {
            return _cfg.Save();
        }
    }
}
