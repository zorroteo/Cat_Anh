using System.Drawing;

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
    }
}