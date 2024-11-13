using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PicChoose
{
    public partial class Form1 : Form
    {
        private string[] _imageFiles;
        private int _currentIndex;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = SetPicPath();
            label1.Text = path;
            DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\ѡ");
            PicPath = directoryInfo.FullName;
            label2.Text = PicPath;
            PicLoad(path);
        }

        private string SetPicPath()
        {
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            string destinationFolder = "";
            DialogResult result = folderDialog.ShowDialog();
            if (result == DialogResult.OK)
            {

                return destinationFolder = folderDialog.SelectedPath;
            }
            return destinationFolder;
        }
        private void PicLoad(string Path)
        {
            // ��ȡ�ļ����е�����ͼƬ�ļ�
            string folderPath = Path; // �޸�Ϊ���Լ���ͼƬ�ļ���·��
            _imageFiles = Directory.GetFiles(folderPath, "*.*")
                                   .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                   .ToArray();

            // ���õ�ǰ����Ϊ��һ��ͼƬ
            if (_imageFiles.Length > 0)
            {
                _currentIndex = 0;
                DisplayImage();
            }
            else
            {
                MessageBox.Show("���ļ���û��ͼƬ��", "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // ��ʾ��ǰͼƬ
        private void DisplayImage()
        {
            if (_imageFiles.Length > 0)
            {
                pictureBox1.ImageLocation = _imageFiles[_currentIndex];
                label3.Text = _imageFiles[_currentIndex];
                label4.Text = "";
                GC.Collect();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_imageFiles.Length > 0)
            {
                _currentIndex = (_currentIndex - 1 + _imageFiles.Length) % _imageFiles.Length; // ѭ����ʾ
                DisplayImage();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_imageFiles.Length > 0)
            {
                _currentIndex = (_currentIndex + 1) % _imageFiles.Length; // ѭ����ʾ
                DisplayImage();
            }
        }

        private void Choose()
        {
            if (_imageFiles.Length > 0)
            {
                string fileName = Path.GetFileName(_imageFiles[_currentIndex]);
                string destinationPath = Path.Combine(PicPath, fileName);
                // �����ļ���Ŀ���ļ���
                try
                {
                    File.Copy(_imageFiles[_currentIndex], destinationPath);
                    label4.Text = $"{_imageFiles[_currentIndex]}ͼƬ�Ѹ��Ƶ� {destinationPath}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"����ʧ��: {ex.Message}", "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            PicPath = SetPicPath();

            label2.Text = PicPath;

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Choose();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                Arguments = PicPath,
                FileName = "explorer.exe"
            };

            Process.Start(startInfo);
        
        }

        public string PicPath { set; get; }
    }
}
