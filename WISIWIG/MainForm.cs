using System;
using System.Linq;
using System.Windows.Forms;

namespace WISIWIG
{
    public partial class MainForm : Form
    {
        private const string File = "D:\\index.html";

        public MainForm()
        {
            InitializeComponent();
        }

        private void editor_TextChanged(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(File, editor.Text);
            webBrowser.Navigate(File);
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();

            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = dlg.FileName;
            string fileText = System.IO.File.ReadAllText(filename);
            editor.Text = fileText;
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();

            if (dlg.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = dlg.FileName;
            System.IO.File.WriteAllText(filename, editor.Text);
        }

        private void закрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(File, editor.Text);
        }

        private void editor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 62)
            {
                e.Handled = true;
                AddCloseTag();
            }
        }

        private void AddCloseTag()
        {
            //get position
            int pos = editor.SelectionStart - 1;
            int posSaved = editor.SelectionStart - 1;
            InsertAtPosition(">", pos + 1);
            //get tag name
            string tagName = "";
            while (pos > -1)
            {
                try
                {
                    if (editor.Text[pos] == '/' && editor.Text[pos - 1] == '<')
                    {
                        editor.SelectionStart = posSaved + 2;
                        return;
                    }
                }
                catch { }

                if (editor.Text[pos] == '<')
                {
                    tagName = editor.Text.Substring(pos, posSaved - pos + 1) + ">";
                    break;
                }

                pos--;
            }
            //insert tag closing
            InsertAtPosition("</" + tagName.Substring(1, tagName.Length - 1), posSaved + 2);
            //set cursor to the right position
            editor.SelectionStart = posSaved + 2;
        }

        void InsertAtPosition(string text, int pos)
        {
            string lastPart = editor.Text.Substring(pos);
            try
            {
                editor.Text = editor.Text.Remove(pos);
            }
            catch { }
            editor.Text += text + lastPart;
        }
    }
}
