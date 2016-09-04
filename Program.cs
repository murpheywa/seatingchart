using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace SeatingChart
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
			// Make sure I can write to c:\temp
			try
			{
				Directory.CreateDirectory(@"c:\temp");
				File.WriteAllText(@"c:\temp\temp.txt", "temp\n");
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new frmSearingChart());
			}
			catch (Exception)
			{
				MessageBox.Show(@"I am sorry.  can't write to c:\temp. I can't go on.", "Can't go on ...", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
        }
    }
}
