using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    public partial class Form3 : Form
    {
        public int TargetIdx { get; private set; }
        public int TargetValue { get; private set; }


        public Form3(int idx)
        {
            InitializeComponent();
            TargetIdx = idx;
            setval.Text = configData.dic[idx].Value.ToString();
            label5.Font = new Font("Consolas", 10F, FontStyle.Regular);
            label5.Text =
@"1 : K    ( -200 ~ 1370 )
2 : K    ( -200.0 ~ 1370.0 )
3 : J    ( -200 ~ 1200 )
4 : J    ( -200.0 ~ 1200.0 )
5 : E    ( -199 ~ 999 )
6 : E    ( -199.0 ~ 999.0 )
7 : T    ( -50 ~ 400 )
8 : T    ( -50.0 ~ 400.0 )
9 : R    ( 0 ~ 1700 )
10: B    ( 0 ~ 1800 )
11: S    ( 0 ~ 1700 )
12: L    ( -199 ~ 900 )
13: L    ( -199.0 ~ 900.0 )
14: N    ( -199 ~ 1300 )
15: U    ( -50 ~ 400 )
16: U    ( -50.0 ~ 400.0 )
17: W    ( 0 ~ 2300 )
18: PLII ( 0 ~ 1300 )";
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            label1.BackColor = SystemColors.GrayText;
        }

        private void registBtn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(setval.Text, out int inputVal))
            {
                if (0 < inputVal && inputVal < 19)
                {
                    TargetValue = inputVal;
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    MessageBox.Show("1 ~ 18 사이의 값을 입력해주세요.");
                }
            }
            else
            {
                MessageBox.Show("올바른 숫자를 입력해주세요.");
            }
        }

        private void setval_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
