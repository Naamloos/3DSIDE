using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _3DSIDE
{
    class Tools
    {
        public static void GenerateExampleProject(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }

            File.Create(path + "/Makefile").Close();

            string fileContents = Properties.Resources.Makefile;

            File.WriteAllText(path + "/Makefile", fileContents);

            Directory.CreateDirectory(path + "/source");

            File.Create(path + "/source/main.c").Close();

            fileContents = Properties.Resources.Main;

            File.WriteAllText(path + "/source/main.c", fileContents);
        }
    }
}
