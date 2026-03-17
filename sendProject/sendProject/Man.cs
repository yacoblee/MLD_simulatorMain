using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sendProject
{
    public class Man
    {
        private Config _cfg;

        public int idx
        {
            get
            {
                return _cfg.Idx;
            }
            set
            {
                _cfg.Idx = value;
            }
        }
        public int value
        {
            get
            {
                return _cfg.value;
            }
            set
            {
                _cfg.value = value;
            }
        }

        public Boolean isValid(int idx, int value)
        {

            if (idx == 0x0101 || idx == 0x0201 || idx == 0x0301 || idx == 0x0401) //PV1~4 수정 (셀 +100 ~ +400) 
            {
                if (1 <= value && value < 16)
                {
                    return true;
                }
            }



            return true;
        }



        public Man(Config cfg)
        {
            _cfg = cfg;
        }


        public void Save()
        {
            _cfg.Save();
        }
    }
}
