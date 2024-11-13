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
            DirectoryInfo directoryInfo = Directory.CreateDirectory(path + "\\选");
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
            // 获取文件夹中的所有图片文件
            string folderPath = Path; // 修改为你自己的图片文件夹路径
            _imageFiles = Directory.GetFiles(folderPath, "*.*")
                                   .Where(file => file.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                                  file.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                                   .ToArray();

            // 设置当前索引为第一个图片
            if (_imageFiles.Length > 0)
            {
                _currentIndex = 0;
                DisplayImage();
            }
            else
            {
                MessageBox.Show("该文件夹没有图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 显示当前图片
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
                _currentIndex = (_currentIndex - 1 + _imageFiles.Length) % _imageFiles.Length; // 循环显示
                DisplayImage();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_imageFiles.Length > 0)
            {
                _currentIndex = (_currentIndex + 1) % _imageFiles.Length; // 循环显示
                DisplayImage();
            }
        }

        private void Choose()
        {
            if (_imageFiles.Length > 0)
            {
                string fileName = Path.GetFileName(_imageFiles[_currentIndex]);
                string destinationPath = Path.Combine(PicPath, fileName);
                // 复制文件到目标文件夹
                try
                {
                    File.Copy(_imageFiles[_currentIndex], destinationPath);
                    label4.Text = $"{_imageFiles[_currentIndex]}图片已复制到 {destinationPath}";
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"复制失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
