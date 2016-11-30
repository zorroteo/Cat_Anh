using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Cat_Anh
{
    public partial class test : DevComponents.DotNetBar.Metro.MetroForm
    {
        private OpenFileDialog file = new OpenFileDialog();
        private static Image img = null;
        private int a = 100;
        private int xp1, yp1, xp2, yp2, xpanel, ypanel, x, y;
        private int cropWidth;
        private int cropHeight;
        private int tmp = 0;
        private Rectangle rect;
        private Bitmap cropBitmap;
        private Pen cropPen = new Pen(Color.Yellow, 1);
        public DashStyle cropDashStyle = DashStyle.DashDot;
        public test()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                DialogResult dialogre = file.ShowDialog();
                if (dialogre == DialogResult.OK)
                {
                    img = Image.FromFile(file.FileName);
                    pictureBox1.Image = img;
                }
            }
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            pictureBox1.MouseHover += new EventHandler(pictureBox1_MouseHover);
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Focus();
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
                    textBox2.Text = a.ToString();
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
                    textBox2.Text = a.ToString();
                }
            }
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image = Su_ly.zoom(img, a); //zoom ảnh
            }
               
        }
       

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e) // kéo giữ chuột
        {
            if (e.Button == MouseButtons.Left)
            {
                if (pictureBox1.Image == null)
                {
                    return;
                }
                pictureBox1.Refresh();
                cropWidth = e.X - xp1;
                cropHeight = e.Y - yp1;

                #region if Chọn button crop
                if (tmp == 1)
                {
                    if (cropWidth > 0 && cropHeight > 0)
                    {
                        pictureBox1.Refresh();
                        Graphics g = pictureBox1.CreateGraphics();
                        g.DrawRectangle(cropPen, xp1, yp1, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(xp1, yp1, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }
                    if (cropWidth < 0 && cropHeight < 0)
                    {
                        pictureBox1.Refresh();
                        Graphics g = pictureBox1.CreateGraphics();
                        g.DrawRectangle(cropPen, e.X, e.Y, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(e.X, e.Y, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }

                    if (cropWidth < 0 && cropHeight > 0)
                    { pictureBox1.Refresh();
                        Graphics g = pictureBox1.CreateGraphics();
                        g.DrawRectangle(cropPen, e.X, yp1, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(e.X, yp1, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }
                    if (cropWidth > 0 && cropHeight < 0)
                    {
                        pictureBox1.Refresh();
                        Graphics g = pictureBox1.CreateGraphics();
                        g.DrawRectangle(cropPen, xp1, e.Y, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        rect = new Rectangle(xp1, e.Y, Math.Abs(e.X - xp1), Math.Abs(e.Y - yp1));//hình chữ nhật tọa độ góc trên bên trái và h,w
                    }
                }
                #endregion

            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e) // Nhấn chuột trái
        {
            if (e.Button == MouseButtons.Left)
            {
                xp1 = e.X;
                yp1 = e.Y;
                Cursor = Cursors.Cross;
                cropPen.DashStyle = DashStyle.DashDotDot;
            }
            pictureBox1.Refresh();
        }


        private void pictureBox1_MouseUp(object sender, MouseEventArgs e) // bỏ nhấn chuột trái
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
                    xpanel = pictureBox1.Location.X - x;
                }
                else
                {
                    xpanel = pictureBox1.Location.X + x;
                }
                if (yp2 - yp1 < 0)
                {
                    ypanel = pictureBox1.Location.Y - y;
                }
                else
                {
                    ypanel = pictureBox1.Location.Y + y;
                }
                pictureBox1.Location = new Point(xpanel, ypanel);
            }

            #endregion Kéo thả ảnh

            //Chon vùng cắt ảnh
            if (tmp == 1)
            {
                Bitmap bit = new Bitmap(pictureBox1.Image, pictureBox1.Width, pictureBox1.Height);
                cropBitmap = new Bitmap(Math.Abs(cropWidth), Math.Abs(cropHeight));
                Graphics g = Graphics.FromImage(cropBitmap);// tao moi do hoa tu hinh anh
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                g.DrawImage(bit, 0, 0, rect, GraphicsUnit.Pixel);   //Vẽ định hình tại vị trí quy định và với kích thước quy định.
                pictureBox2.Image = cropBitmap;
                pictureBox1.Width = cropBitmap.Width;
                pictureBox1.Height = cropBitmap.Height;
                cropBitmap.Save("e:\\abc.jpg");
            }
        }
        
        private void buttonX1_Click(object sender, EventArgs e)
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
    }
}