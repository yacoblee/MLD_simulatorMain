using copyApp.Conf;
using copyApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace copyApp
{
    internal static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ProtocolFIle config = ProtocolFIle.Load();
            MainModel model = new MainModel(config);

            Application.Run(new MainView(model));



            SerialComm.Instance.Dispose();
        }
    }
}
