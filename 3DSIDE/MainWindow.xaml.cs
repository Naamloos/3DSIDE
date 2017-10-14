using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _3DSIDE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<string> Projects = new List<string>();
        public static string CurrentFilePath = "";
        public static List<string> Extensions;
        public static List<string> Compilables;

        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists("project"))
            {
                Directory.CreateDirectory("project");
                Tools.GenerateExampleProject("project");
            }
            Extensions = new List<string>()
            {
                ".c",
                ".h",
                "",
                ".md",
                ".txt",
                ".xml",
                ".sh",
                ".bat",
                ".py",
                ".cs",
                ".cpp",
                ".json"
            };

            Compilables = new List<string>()
            {
                ".3dsx",
                ".cia",
                "clean"
            };

            foreach(string s in Compilables)
            {
                comboBox.Items.Add(s);
                comboBox.SelectedIndex = 0;
            }

            LoadProject(Directory.GetCurrentDirectory() + "/project");
        }

        public static void ShowOpenWithDialog(string path)
        {
            var args = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "shell32.dll");
            args += ",OpenAs_RunDLL " + path;
            Process.Start("rundll32.exe", args);
        }

        public void LoadProject(string path)
        {
            Directory.SetCurrentDirectory(path);
            textEditor.IsEnabled = false;
            listView.Items.Clear();
            foreach(string f in Directory.GetFiles(path))
            {
                listView.Items.Add(f.Substring(path.Length));
            }
            foreach (string d in Directory.GetDirectories(path, "*", SearchOption.AllDirectories))
            {
                listView.Items.Add("<" + d.Substring(path.Length) + ">");
                foreach (string f in Directory.GetFiles(d))
                {
                    listView.Items.Add(f.Substring(path.Length));
                }
            }
        }

        private void AppExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void listView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string item = (string)e.AddedItems[0];
                if (!item.Contains("<"))
                {
                    CurrentFilePath = Directory.GetCurrentDirectory() + item;
                    if (Extensions.Contains(System.IO.Path.GetEx‌​tension(Directory.GetCurrentDirectory() + item)))
                        textEditor.Text = File.ReadAllText(Directory.GetCurrentDirectory() + item);
                    else
                    {
                        ShowOpenWithDialog(Directory.GetCurrentDirectory() + item);
                        return;
                    }
                    try
                    {
                        textEditor.SyntaxHighlighting = ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Inst‌​ance.GetDefinitionBy‌​Extension(System.IO.Path.GetEx‌​tension(CurrentFilePath));
                    }
                    catch (Exception)
                    {

                    }
                    textEditor.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(CurrentFilePath, textEditor.Text);
            System.Windows.Forms.MessageBox.Show("File saved!");
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    LoadProject(fbd.SelectedPath);
                }
            }
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.MessageBox.Show("3DSIDE is a Nintendo 3DS focused IDE, made by Naamloos");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if((string)comboBox.SelectedItem == ".3dsx")
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "/Makefile"))
                {
                    Compiler.Make3DSX(Directory.GetCurrentDirectory());
                    LoadProject(Directory.GetCurrentDirectory());
                }
                else if (Directory.GetCurrentDirectory().Contains(' '))
                    System.Windows.Forms.MessageBox.Show("Please move to a folder that doesnt contain spaces, Make doesn't like those!");
                else
                    System.Windows.Forms.MessageBox.Show("Project Directory doesn't contain a makefile!");
            }
            else if ((string)comboBox.SelectedItem == "clean")
            {
                if (File.Exists(Directory.GetCurrentDirectory() + "/Makefile"))
                {
                    Compiler.MakeClean(Directory.GetCurrentDirectory());
                    LoadProject(Directory.GetCurrentDirectory());
                }
                else if (Directory.GetCurrentDirectory().Contains(' '))
                    System.Windows.Forms.MessageBox.Show("Please move to a folder that doesnt contain spaces, Make doesn't like those!");
                else
                    System.Windows.Forms.MessageBox.Show("Project Directory doesn't contain a makefile!");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Build method not supported yet!");
            }
        }

        private void GenerateEmpty_Click(object sender, RoutedEventArgs e)
        {
            Tools.GenerateExampleProject(Directory.GetCurrentDirectory());
            System.Windows.Forms.MessageBox.Show("Generated example project!");
            LoadProject(Directory.GetCurrentDirectory());
        }

        private void Docs_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://smealum.github.io/ctrulib/");
        }
    }
}
