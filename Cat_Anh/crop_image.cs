using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Cat_Anh
{
    public partial class crop_image : DevComponents.DotNetBar.OfficeForm
    {
        private OpenFileDialog file = new OpenFileDialog();
        private static Image img = null;

        /// <summary>
        /// Biến lưu giá trị zoom
        /// </summary>
        private int a = 100;

        private int x1_picture, y1_picture, x2_picture, y2_picture, xloca_img, yloca_img, x, y;

        private Bitmap cropBitmap;
        private Pen cropPen = new Pen(Color.Yellow, 1);
        public DashStyle cropDashStyle = DashStyle.DashDot;

        /// <summary>
        /// Chiều dài ảnh đã crop
        /// </summary>
        private int cropWidth;

        /// <summary>
        /// Chiều rộng ảnh đã crop
        /// </summary>
        private int cropHeight;

        /// <summary>
        /// Biến lưu sự kiện click button crop, t=1: crop; t=0: bỏ crop
        /// </summary>
        private int crop_click = 0;

        /// <summary>
        /// tọa độ đỉnh trên cùng để crop
        /// </summary>
        private int cropx, cropy;

        //Image[] al = new Image[100];
        //private int t;
        public crop_image()
        {
            InitializeComponent();
        }

        private void pb_image_MouseMove(object sender, MouseEventArgs e)//di chuyển chuột
        {
            int x, y;
            lbX.Text = " x: " + e.X.ToString();//In ra màn hình tọa độ điểm x
            lbY.Text = " y: " + e.Y.ToString(); ;//In ra màn hình tọa độ điểm y
            if (e.Button == MouseButtons.Left)//kéo giữ chuột trái
            {
                if (pb_image.Image == null)
                {
                    return;
                }
                x = e.X;
                y = e.Y;
                if (e.X < 0) x = 0;
                if (e.X > img.Width) x = img.Width - 1;
                if (e.Y < 0) y = 0;
                if (e.Y > img.Height) y = img.Height - 1;

                pb_image.Refresh();
                cropWidth = x - x1_picture;
                cropHeight = y - y1_picture;

                #region if Chọn button crop

                if (crop_click == 1)
                {
                    if (e.X < 0)
                    {
                    }
                    if (cropWidth > 0 && cropHeight > 0)//x > x1_picture && y > y1_picture
                    {
                        Su_ly.Ve_hinh_cn(pb_image, x1_picture, y1_picture, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        cropx = x1_picture; cropy = y1_picture;
                    }
                    if (cropWidth < 0 && cropHeight < 0)//x < x1_picture && y < y1_picture
                    {
                        Su_ly.Ve_hinh_cn(pb_image, x, y, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        cropx = x; cropy = y;
                    }

                    if (cropWidth < 0 && cropHeight > 0)//x < x1_picture && y > y1_picture
                    {
                        Su_ly.Ve_hinh_cn(pb_image, x, y1_picture, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        cropx = x; cropy = y1_picture;
                    }
                    if (cropWidth > 0 && cropHeight < 0)//x > x1_picture && y < y1_picture
                    {
                        Su_ly.Ve_hinh_cn(pb_image, x1_picture, y, Math.Abs(cropWidth), Math.Abs(cropHeight));
                        cropx = x1_picture; cropy = y;
                    }
                }

                #endregion if Chọn button crop
            }
        }

        /// <summary>
        /// Button save ảnh crop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_save_Click(object sender, EventArgs e)
        {
            Su_ly.LuuFileAnh(cropBitmap);
        }

        /// <summary>
        /// Menu: open image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult dialogre = file.ShowDialog();
            if (dialogre == DialogResult.OK)
            {
                img = Image.FromFile(file.FileName);
                pb_image.Image = img;
                // al[t+1]=img; t = al.Length;
            }
        }

        /// <summary>
        /// Menu: new image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            b = 0;
            img = Image.FromFile(@"img/1480552501_Image_-_Google_Docs.png", true);
            pb_image.Image = img;
            pb_image.Location = new Point(386, 254);
        }

        /// <summary>
        /// Button crop ảnh sau khi chon vùng crop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_cut_Click(object sender, EventArgs e)
        {
            pb_image.Image = cropBitmap;
            img = cropBitmap;
            // al[t + 1] = img; t = al.Length;
        }

        private void crop_image_Load(object sender, EventArgs e)
        {
            if (crop_click == 0)
            {
                btn_crop.BackColor = Color.Orange;
            }
            else
            {
                btn_crop.BackColor = Color.DarkGray;
            }
        }

        private int undo;

        private void btn_undo_Click(object sender, EventArgs e)
        {
            //undo = t-1;
            //pb_image.Image = al[undo];
            //undo = undo - 1;
        }

        /// <summary>
        /// Button save ảnh crop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnsave_Click(object sender, EventArgs e)
        {
            Su_ly.LuuFileAnh(cropBitmap);
        }

        /// <summary>
        /// Button chọn lệnh crop khi kéo thả chuột trái
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_crop_Click(object sender, EventArgs e)
        {
            // chưa chọn buton chọn crop thì crop_click=0 else crop_click =1
            if (crop_click == 0)
            {
                btn_crop.BackColor = Color.Orange;
                crop_click = 1;
            }
            else
            {
                crop_click = 0;
                btn_crop.BackColor = Color.DarkGray;
            }
        }

        /// <summary>
        /// Biến xác định đã click vào chọn ảnh hay chưa
        /// </summary>
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
                    pb_image.Location = new Point(0, 0);
                    // al[t + 1] = img; t = al.Length;
                    b = 1;
                }
                else
                {
                    img = Image.FromFile(@"img/1480552501_Image_-_Google_Docs.png", true);
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

        private void pb_image_MouseUp(object sender, MouseEventArgs e)//Khi thả chuột trái
        {
            x2_picture = e.X;
            y2_picture = e.Y;

            #region Kéo thả ảnh

            if (crop_click == 0)
            {
                //kéo thả ảnh ở vị tri mới
                // thục chất là thay đổi vi trí của picturebox trong panel
                x = x2_picture - x1_picture;
                y = y2_picture - y1_picture;
                xloca_img = pb_image.Location.X + x;
                yloca_img = pb_image.Location.Y + y;
                pb_image.Location = new Point(xloca_img, yloca_img);
            }

            #endregion Kéo thả ảnh

            //Chon vùng cắt ảnh
            if (crop_click == 1)
            {
                //pb_image.Image = Su_ly.crop(cropx, cropy, Math.Abs(cropWidth), Math.Abs(cropHeight), pb_image.Image);
                cropBitmap = new Bitmap(Su_ly.crop(cropx, cropy, Math.Abs(cropWidth), Math.Abs(cropHeight), pb_image.Image));
            }
        }

        private void pb_image_MouseDown(object sender, MouseEventArgs e)//Khi nhấn chuột trái
        {
            if (e.Button == MouseButtons.Left)
            {
                x1_picture = e.X;
                y1_picture = e.Y;
                Cursor = Cursors.Cross;
                cropPen.DashStyle = DashStyle.DashDotDot;
            }

            Cursor = Cursors.Arrow;
            pb_image.Refresh();
        }
    }
}