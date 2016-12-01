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
    }
}