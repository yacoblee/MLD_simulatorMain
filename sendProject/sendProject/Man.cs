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

        private Dictionary<int, Func<int, bool>> _validationRules = new Dictionary<int, Func<int, bool>>();
        public int[] svGroup = { 102, 202, 302, 402, 6, 7, 8, 9, 109 };
        public int[] pvGroup = { 101, 201, 301, 401, 1, 2, 3, 4, 209, 309, 409 };


        public Man(Config cfg)
        {
            InitValidationRules();
            _cfg = cfg;
        }


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

        /*        public Boolean isValid(int idx, int value)
                {

                    if (idx == 102 || idx == 202 || idx == 302 || idx == 402 || idx == 6 || idx == 7 || idx == 8 || idx == 9
                        || idx == 109) //SV1~4 수정 (셀 +100 ~ +400) 
                    {
                        if (1 <= value && value <= 100)
                        {
                            return true;
                        }
                    }

                    if (idx == 101 || idx == 201 || idx == 301 || idx == 401 
                        || idx == 1 || idx == 2 || idx == 3 || idx == 4 || 
                        idx == 109 || idx == 209 || idx == 309 || idx == 409 ) //PV1~4 수정 (셀 +100 ~ +400) 
                    {
                        if (1 <= value && value < 16)
                        {
                            return true;
                        }
                    }

                    return true;
                }
         */
        private void InitValidationRules()
        {

            preValid(svGroup, val => val >= 0 && val <= 100);
            preValid(pvGroup, val => val >= 1 && val <= 18);

        }

        private void preValid(int[] indices, Func<int, bool> rule)
        {
            foreach (int idx in indices)
            {
                _validationRules[idx] = rule;
            }
        }

        public Boolean isValid(int idx, int value)
        {
            if (_validationRules.ContainsKey(idx))
            {
                return _validationRules[idx](value);
            }

            return true;
        }

        public void SetValue(int idx, int value)
        {
            _cfg.ParamData[idx] = value;
        }

        public void Save()
        {
            _cfg.Save();
        }
    }
}
