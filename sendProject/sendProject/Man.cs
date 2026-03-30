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
            _cfg = cfg;
            InitValidationRules();
            InitDefaultValues();
        }


        private void InitDefaultValues()
        {
            if (_cfg.ParamData.Count == 0)
            {
                foreach (int idx in svGroup)
                {
                    _cfg.ParamData[idx] = 1;
                }

                foreach (int idx in pvGroup)
                {
                    _cfg.ParamData[idx] = 1;
                }

                _cfg.Save();
            }
        }

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
        public Dictionary<int, int> GetAllParams()
        {
            return _cfg.ParamData;
        }

        public bool Save()
        {
            return _cfg.Save();
        }
    }
}
