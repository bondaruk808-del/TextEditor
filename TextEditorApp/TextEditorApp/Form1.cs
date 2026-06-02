using System;
using System.Windows.Forms;

namespace TextEditorApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Очистити поточний текст і створити новий файл?",
                    "Новий документ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No) return;
            }
            richTextBox1.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(richTextBox1.Text))
            {
                DialogResult result = MessageBox.Show(
                    "Ви впевнені, що хочете закрити Brotato Pad? Всі незбережені дані будуть втрачені.",
                    "Увага!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No) e.Cancel = true;
            }
            base.OnFormClosing(e);
        }
    }
}