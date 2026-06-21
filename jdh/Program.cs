using jdh.Data;
using jdh.Model;
using jdh.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jdh
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

            SerialConfig config = SerialConfig.Load();
            SerialModel model = new SerialModel(config);
            SerialView view = new SerialView(model);
            Application.Run(view);
        }
    }
}
