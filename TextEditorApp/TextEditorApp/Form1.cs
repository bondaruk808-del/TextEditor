using System;
using System.Windows.Forms;

namespace TextEditorApp
{
    public partial class Form1 : Form
    {
        private bool isFileChanged = false;
        private string currentFileName = "Без імені";

        public Form1()
        {
            InitializeComponent();
            UpdateFormTitle();
        }

        private void UpdateFormTitle()
        {
            string asterisk = isFileChanged ? "*" : "";
            this.Text = $"{asterisk}{currentFileName} — Brotato Pad";
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFileChanged)
            {
                DialogResult result = MessageBox.Show(
                    "Очистити поточний текст і створити новий файл? Незбережені зміни будуть втрачені.",
                    "Новий документ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.No) return;
            }

            richTextBox1.Clear();
            currentFileName = "Без імені";
            isFileChanged = false;
            UpdateFormTitle();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (!isFileChanged)
            {
                isFileChanged = true;
                UpdateFormTitle();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (isFileChanged)
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