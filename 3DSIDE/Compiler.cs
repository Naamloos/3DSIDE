using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DSIDE
{
    public class Compiler
    {
        public static void Make3DSX(string makefilepath)
        {
            Directory.SetCurrentDirectory(makefilepath);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C make";
            process.StartInfo.WorkingDirectory = makefilepath;
            process.Start();
            System.Windows.Forms.MessageBox.Show(".3DSX built!");
        }

        public static void MakeClean(string makefilepath)
        {
            Directory.SetCurrentDirectory(makefilepath);
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/C make clean";
            process.StartInfo.WorkingDirectory = makefilepath;
            process.Start();
            System.Windows.Forms.MessageBox.Show("Directory is clean!");
        }
    }
}
