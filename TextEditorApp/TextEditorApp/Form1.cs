using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;
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

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isFileChanged)
            {
                DialogResult result = MessageBox.Show(
                    "Поточний файл має незбережені зміни. Продовжити відкриття іншого файлу?",
                    "Увага!",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.No) return;
            }

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|Портативний документ (*.pdf)|*.pdf|Усі файли (*.*)|*.*";
                openFileDialog.Title = "🥔 Відкрити файл у Brotato Pad";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string extension = Path.GetExtension(filePath).ToLower();

                    try
                    {
                        if (extension == ".rtf")
                        {
                            richTextBox1.LoadFile(filePath, RichTextBoxStreamType.RichText);
                        }
                        else if (extension == ".pdf")
                        {
                            richTextBox1.Text = File.ReadAllText(filePath);
                            MessageBox.Show("PDF файли відкриваються в режимі зчитування текстових даних.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            richTextBox1.Text = File.ReadAllText(filePath);
                        }

                        currentFileName = Path.GetFileName(filePath);
                        isFileChanged = false;
                        UpdateFormTitle();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при відкритті файлу: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Rich Text Format (*.rtf)|*.rtf|Портативний документ (*.pdf)|*.pdf";
                saveFileDialog.Title = "🥔 Зберегти файл у Brotato Pad";
                saveFileDialog.DefaultExt = "txt";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;
                    string extension = Path.GetExtension(filePath).ToLower();

                    if (extension == ".rtf")
                    {
                        richTextBox1.SaveFile(filePath, RichTextBoxStreamType.RichText);
                    }
                    else if (extension == ".pdf")
                    {
                        SaveAsPdf(filePath);
                    }
                    else
                    {
                        File.WriteAllText(filePath, richTextBox1.Text);
                    }

                    currentFileName = Path.GetFileName(filePath);
                    isFileChanged = false;
                    UpdateFormTitle();

                    MessageBox.Show("Файл успішно збережено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SaveAsPdf(string filePath)
        {
            using (PrintDocument printDocument = new PrintDocument())
            {
                printDocument.PrinterSettings.PrinterName = "Microsoft Print to PDF";
                printDocument.PrinterSettings.PrintToFile = true;
                printDocument.PrinterSettings.PrintFileName = filePath;

                printDocument.PrintPage += (s, ev) =>
                {
                    ev.Graphics.DrawString(
                        richTextBox1.Text,
                        richTextBox1.Font,
                        System.Drawing.Brushes.Black,
                        ev.MarginBounds.Left,
                        ev.MarginBounds.Top
                    );
                };

                printDocument.Print();
            }
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

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FontDialog fontDialog = new FontDialog())
            {
                if (richTextBox1.SelectionFont != null)
                {
                    fontDialog.Font = richTextBox1.SelectionFont;
                }

                if (fontDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.SelectionFont = fontDialog.Font;
                }
            }
        }

        private void textColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = richTextBox1.SelectionColor;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.SelectionColor = colorDialog.Color;
                }
            }
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.Color = richTextBox1.SelectionBackColor;

                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.SelectionBackColor = colorDialog.Color;
                }
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont != null)
            {
                Font currentFont = richTextBox1.SelectionFont;
                FontStyle newFontStyle = currentFont.Bold ? currentFont.Style & ~FontStyle.Bold : currentFont.Style | FontStyle.Bold;
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (richTextBox1.SelectionFont != null)
            {
                Font currentFont = richTextBox1.SelectionFont;
                FontStyle newFontStyle = currentFont.Italic ? currentFont.Style & ~FontStyle.Italic : currentFont.Style | FontStyle.Italic;
                richTextBox1.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
            }
        }
    }
}