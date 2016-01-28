using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace autoprint
{
    public partial class Form1 : Form
    {
        Dictionary<string, List<string>> contents = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            printDocument1.DefaultPageSettings.Landscape = false;
        }

        private void ReadDocument()
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowDialog();
            contents = new Dictionary<string, List<string>>();
            DirectoryInfo di = new DirectoryInfo(fbd.SelectedPath);
            foreach (DirectoryInfo folder in di.GetDirectories())
            {
                List<string> tmp = new List<string>();
                foreach (FileInfo f in folder.GetFiles("*.jpg"))
                {
                    tmp.Add(f.FullName); 
                }
                if (tmp.Count>0)
                    contents.Add(folder.FullName, tmp);
            }
            
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {

            if (contents.Count > 0)
            {
                Rectangle m = e.MarginBounds;
                try
                {
                    bool restart = false;
                    foreach (string path in contents.Keys)
                    {
                        for (int i = 1; i <= contents[path].Count; i++)
                        {
                            if (i > 4 && i % 4 == 1)
                            {
                                e.HasMorePages = true;
                                contents[path].RemoveRange(0, 4);
                                restart = true;
                                break;
                            }
                            System.Drawing.Image img = System.Drawing.Image.FromFile(contents[path][i - 1]);
                            switch (i % 4)
                            {
                                case 1:
                                    e.Graphics.DrawImage(img, 50, 50, m.Width / 2, m.Height / 2);
                                    break;
                                case 2:
                                    e.Graphics.DrawImage(img, m.Width / 2 + 80, 50, m.Width / 2, m.Height / 2);
                                    break;
                                case 3:
                                    e.Graphics.DrawImage(img, 50, m.Height / 2 + 100, m.Width / 2, m.Height / 2);
                                    break;
                                case 0:
                                    e.Graphics.DrawImage(img, m.Width / 2 + 80, m.Height / 2 + 100, m.Width / 2, m.Height / 2);
                                    break;
                            }
                        }
                        if (contents[path].Count == 0)
                            contents.Remove(path);
                        if (!restart)
                           e.HasMorePages = true;
                    }
                   
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
                finally
                {

                }
            }
            else
                e.HasMorePages = false;

        }

        private void useswitch(int i)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadDocument();
            
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
            
        }
    }
}
