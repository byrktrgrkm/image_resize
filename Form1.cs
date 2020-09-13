using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Image_Resize
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }
        OpenFileDialog folderBrowser;
        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowser = new OpenFileDialog();
            // Set validate names and check file exists to false otherwise windows will
            // not let you select "Folder Selection."
            folderBrowser.ValidateNames = false;
            folderBrowser.CheckFileExists = false;
            folderBrowser.CheckPathExists = true;
            // Always default to Folder Selection.
            folderBrowser.FileName = "Folder Selection.";
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {

                var list = Directory.GetFiles(Path.GetDirectoryName(folderBrowser.FileName), "*.*", SearchOption.AllDirectories).ToList();
                if(list.Count > 0)
                {
                    MessageBox.Show("Klasör seçildi");
                }

            }
            else
            {
                MessageBox.Show("Klasör seçilemedi!");
            }


        }
        public Image resizeImage(int newWidth, int newHeight, string stPhotoPath)
        {
            Image imgPhoto = Image.FromFile(stPhotoPath);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;

            //Consider vertical pics
            if (sourceWidth < sourceHeight)
            {
                int buff = newWidth;

                newWidth = newHeight;
                newHeight = buff;
            }

            int sourceX = 0, sourceY = 0, destX = 0, destY = 0;
            float nPercent = 0, nPercentW = 0, nPercentH = 0;

            nPercentW = ((float)newWidth / (float)sourceWidth);
            nPercentH = ((float)newHeight / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((newWidth -
                          (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((newHeight -
                          (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);


            Bitmap bmPhoto = new Bitmap(newWidth, newHeight,
                          System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                         imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Transparent);
            
            grPhoto.InterpolationMode =
                System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            imgPhoto.Dispose();
            return bmPhoto;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string folderPath = Path.GetDirectoryName(folderBrowser.FileName);
            MessageBox.Show("Başlatıldı");
            var list = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
            .ToList();
            string[] filters = { ".jpg", ".png", ".jpeg" };
            if (list.Count > 0)
            {
                string npath = folderPath + "\\" + "resize_"+ DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                if (!Directory.Exists(npath)) Directory.CreateDirectory(npath);


                foreach (String item in list)
                {
                   
                    if (Array.IndexOf(filters, Path.GetExtension(item)) > -1)
                    {

                        string filename = Path.GetFileName(item);
                        Image i = resizeImage(Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox2.Text), item);
                        i.Save(npath + "\\" + filename);
                    }

                }

                MessageBox.Show("Tamamlandı");

            }
        }
    }
}
