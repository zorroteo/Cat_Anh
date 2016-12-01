using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Drawing.Drawing2D;
using System.IO;

namespace Cat_Anh
{
    public partial class crop_image : DevComponents.DotNetBar.OfficeForm
    {
        private OpenFileDialog file = new OpenFileDialog();
        private static Image img = null;
        private int a = 100;
        private int xp1, yp1, xp2, yp2, xpanel, ypanel, x, y;
        private int cropWidth;
        private Bitmap cropBitmap;
        private Pen cropPen = new Pen(Color.Yellow, 1);
        public DashStyle cropDashStyle = DashStyle.DashDot;
        private int cropHeight;
        private int tmp = 0;
        private Rectangle rect;

        private void pb_image_MouseMove(object sender, MouseEventArgs e)
        {
            lbX.Text =" x: " + e.X.ToString();
            lbY.Text =" y: "+  e.Y.ToString();
            if (e.Button == MouseButtons.Left)
            {
                if (pb_image.Image == null)
                {
                    return;
                }
                pb_image.Refresh();
                cropWidth = e.X - xp1;
                cropHeight = e.Y - yp1;

                #region if Chọn button crop
                if (tmp == 1)
                {
                    if (cropWidth > 0 && cropHeight > 0)
                    {
                        pb_image.Refresh();
                        Graphics g = pb_image.CreateGraphics();
                        g.DrawRectangle(cropPen, xp1, yp1, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(xp1, yp1, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }
                    if (cropWidth < 0 && cropHeight < 0)
                    {
                        pb_image.Refresh();
                        Graphics g = pb_image.CreateGraphics();
                        g.DrawRectangle(cropPen, e.X, e.Y, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(e.X, e.Y, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }

                    if (cropWidth < 0 && cropHeight > 0)
                    {
                        pb_image.Refresh();
                        Graphics g = pb_image.CreateGraphics();
                        g.DrawRectangle(cropPen, e.X, yp1, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(e.X, yp1, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }
                    if (cropWidth > 0 && cropHeight < 0)
                    {
                        pb_image.Refresh();
                        Graphics g = pb_image.CreateGraphics();
                        g.DrawRectangle(cropPen, xp1, e.Y, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(xp1, e.Y, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }
                }
                #endregion

            }
        } // giu keo chuot trai


        private void pb_image_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                xp1 = e.X;
                yp1 = e.Y;
                Cursor = Cursors.Cross;
                cropPen.DashStyle = DashStyle.DashDotDot;
            }

            Cursor = Cursors.Arrow;
            pb_image.Refresh();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Su_ly.LuuFileAnh(cropBitmap);
        }

        private void crop_image_Load(object sender, EventArgs e)
        {
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogre = file.ShowDialog();
            if (dialogre == DialogResult.OK)
            {
                img = Image.FromFile(file.FileName);
                pb_image.Image = img;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            b = 0;
            img = Image.FromFile(@"C:\Users\zorro\documents\visual studio 2015\Projects\Cat_Anh\Cat_Anh\img\1480552501_Image_-_Google_Docs.png", true);
            pb_image.Image = img;
            pb_image.Location=new Point(386, 254);
        }

        private void btn_cut_Click(object sender, EventArgs e)
        {
            pb_image.Image = cropBitmap;
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            Su_ly.LuuFileAnh(cropBitmap);
        }

        private void pb_image_MouseUp(object sender, MouseEventArgs e)
        {
            xp2 = e.X;
            yp2 = e.Y;

            #region Kéo thả ảnh

            if (tmp == 0)
            {
                //kéo thả ảnh ở vị tri mới
                // thục chất là thay đổi vi trí của pb trong panel
                x = Math.Abs(xp2 - xp1);
                y = Math.Abs(yp2 - yp1);
                if (xp2 - xp1 < 0)
                {
                    xpanel = pb_image.Location.X - x;
                }
                else
                {
                    xpanel = pb_image.Location.X + x;
                }
                if (yp2 - yp1 < 0)
                {
                    ypanel = pb_image.Location.Y - y;
                }
                else
                {
                    ypanel = pb_image.Location.Y + y;
                }
                pb_image.Location = new Point(xpanel, ypanel);
            }

            #endregion Kéo thả ảnh
            //Chon vùng cắt ảnh
            if (tmp == 1)
            {
                Bitmap bit = new Bitmap(pb_image.Image, pb_image.Width, pb_image.Height);
                cropBitmap = new Bitmap(Math.Abs(cropWidth), Math.Abs(cropHeight));
                Graphics g = Graphics.FromImage(cropBitmap);// tao moi do hoa tu hinh anh
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel);   //Vẽ định hình tại vị trí quy định và với kích thước quy định.

                //pb_image.Image = cropBitmap;
            }
        }

        private void btn_crop_Click(object sender, EventArgs e)
        {
            // chưa chọn buton vẽ thì tmp=0 else tmp =1
            if (tmp == 0)
            {
                tmp = 1;
            }
            else
            {
                tmp = 0;
            }
        }

        public crop_image()
        {
            InitializeComponent();
        }
        private int b = 0;
        private void pb_image_Click(object sender, EventArgs e)
        {
            if (b == 0)
            {
                pb_image.Image = null;
            }
            if (pb_image.Image == null)
            {
                DialogResult dialogre = file.ShowDialog();
                if (dialogre == DialogResult.OK)
                {
                    img = Image.FromFile(file.FileName);
                    pb_image.Image = img;
                    b = 1;
                }
                else
                {
                    img = Image.FromFile(@"C:\Users\zorro\documents\visual studio 2015\Projects\Cat_Anh\Cat_Anh\img\1480552501_Image_-_Google_Docs.png", true);
                    pb_image.Image = img;
                    pb_image.Location = new Point(386, 254);
                }
            }
            pb_image.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            pb_image.MouseHover += new EventHandler(pictureBox1_MouseHover);
        }
        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pb_image.Focus();
        }

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e) // biến a dùng để lưu giá trị zoom
        {

            if (e.Delta > 0 && a == 200)
            {
                return;
            }
            else
            {
                if (e.Delta > 0 && a >= 5 && a < 200)
                {
                    a += 5;
                }
            }
            if (e.Delta < 0 && a == 5)
            {
                return;
            }
            else
            {
                if (e.Delta < 0 && a > 5 && a <= 200)
                {
                    a -= 5;
                }
            }
            if (pb_image.Image != null)
            {
                pb_image.Image = Su_ly.zoom(img, a); //zoom ảnh
            }

        }
    }
}