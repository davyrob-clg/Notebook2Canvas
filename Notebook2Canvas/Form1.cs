using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notebook2Canvas
{
    public partial class Form1 : Form
    {
        String jsonExportFile;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Gracefully close the application
            Application.Exit();
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Select a file";
                openFileDialog.Filter = "JSON Files (*.json)|*.json|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                //openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                openFileDialog.InitialDirectory = ".";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Display file path
                        textBox1.Text = openFileDialog.FileName;

                        // Read and display file content
                        String content = File.ReadAllText(openFileDialog.FileName);
                        richTextBox1.Text = content;

                        jsonExportFile = openFileDialog.FileName;

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void rjButton3_Click(object sender, EventArgs e)
        {
            JsonToTextConverter jsonToTextConverter = new JsonToTextConverter();

            String textFile = jsonToTextConverter.Convert(jsonExportFile, "output.md");

            richTextBox2.Text = textFile;


        }
    }
}


        