using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Cat_Anh
{
    internal class Su_ly
    {
        static public Image zoom(Image img, int a)
        {
            Bitmap tamp = new Bitmap(img);
            int w, h;
            w = tamp.Width * a / 100;
            h = tamp.Height * a / 100;
            Bitmap btm = new Bitmap(w, h);
            Graphics grap = Graphics.FromImage((Image)btm);
            grap.DrawImage(img, 0, 0, w, h);
            grap.Dispose();
            return (Image)btm;
        }

        public static void LuuFileAnh(System.Drawing.Image img)
        {
            SaveFileDialog s = new SaveFileDialog();
            s.FileName = "Image";// mac dinh file se la anh
            s.DefaultExt = ".Jpg";//mac dinh la jpg
            s.Filter = "Image (.jpg)|.jpg";//loc voi duoi  jpg
            if (s.ShowDialog() == DialogResult.OK)
            {
                //Luu anh
                string filename = s.FileName;
                FileStream fstream = new FileStream(filename, FileMode.Create);
                img.Save(fstream, System.Drawing.Imaging.ImageFormat.Jpeg);
                fstream.Close();
            }
        }

        public static Image crop(int x, int y, int crop_w, int crop_h, Image img)
        {
            Bitmap temp = new Bitmap(img);
            int a1 = temp.Height;
            int b1 = temp.Width;
            Bitmap btm = new Bitmap(crop_w, crop_h);
            for (int i = x; i < crop_w + x; i++)
            {
                for (int j = y; j < crop_h + y; j++)
                {
                    int g = temp.GetPixel(i, j).G;
                    int r = temp.GetPixel(i, j).R;
                    int b = temp.GetPixel(i, j).B;
                    btm.SetPixel(i - x, j - y, Color.FromArgb(r, g, b));
                }
            }
            return btm;
        }

        /// <summary>
        /// hình chữ nhật tọa độ góc trên bên trái và h,w
        /// </summary>
        private static Rectangle rect;

        private static Pen cropPen = new Pen(Color.Yellow, 1);

        public static Rectangle Ve_hinh_cn(PictureBox pb_image, int x, int y, int width, int height)
        {
            pb_image.Refresh();
            Graphics g = pb_image.CreateGraphics();
            g.DrawRectangle(cropPen, x, y, width, height);
            rect = new Rectangle(x, y, width, height);//hình chữ nhật tọa độ góc trên bên trái và h,w
            return rect;
        }
    }
}