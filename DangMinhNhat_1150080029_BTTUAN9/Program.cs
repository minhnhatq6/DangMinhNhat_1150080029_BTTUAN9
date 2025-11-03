using Lab7_Winform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DangMinhNhat_1150080029_BTTUAN9
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // Application.Run(new frmThucHanh1());
            //Application.Run(new frmThucHanh2());
            //Application.Run(new frmThucHanh3());
            //Application.Run(new frmThucHanh4());
            Application.Run(new BTLAMTHEM());
        }
    }
}
