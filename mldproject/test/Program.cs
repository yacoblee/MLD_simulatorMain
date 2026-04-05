using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace test
{
    internal static class Program
    {

        public static TcpConnect MainTcp { get; private set; }
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            
            MainTcp = new TcpConnect();
            SettingCon cfg = SettingCon.Load();
            Man man = new Man(cfg);

            Application.Run(new Form1());

        }
    }
}
