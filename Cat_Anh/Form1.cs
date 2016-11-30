using System;
using System.Drawing;
using System.Windows.Forms;

namespace Cat_Anh
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private OpenFileDialog file = new OpenFileDialog();

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private static Image img = null;

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            DialogResult dialogre = file.ShowDialog();
            if (dialogre == DialogResult.OK)
            {
                img = Image.FromFile(file.FileName);
                pictureBox1.Image = img;
            }
            pictureBox1.MouseWheel += new MouseEventHandler(pictureBox1_MouseWheel);
            pictureBox1.MouseHover += new EventHandler(pictureBox1_MouseHover);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int zoom;
            //Image img = pictureBox1.Image;
            zoom = Convert.ToInt32(textBox1.Text);
            Bitmap tamp = new Bitmap(img);
            int w, h;
            w = tamp.Width * zoom / 100;
            h = tamp.Height * zoom / 100;
            Bitmap btm = new Bitmap(w, h);
            Graphics grap = Graphics.FromImage((Image)btm);
            grap.DrawImage(img, 0, 0, w, h);
            grap.Dispose();
            //return the image
            pictureBox1.Image = (Image)btm;
        }

        private void hScrollBarAdv1_Scroll(object sender, ScrollEventArgs e)
        {
            int d, tp;
            int zoom;
            tp = hScrollBarAdv1.Value / 5;
            d = hScrollBarAdv1.Value % 5;
            if (d < 2)
                zoom = tp * 5;
            else
                zoom = tp * 5 + 5;
            //  Image img = pictureBox1.Image;
            Bitmap tamp = new Bitmap(img);
            int w, h;
            w = tamp.Width * zoom / 100;
            h = tamp.Height * zoom / 100;
            Bitmap btm = new Bitmap(w, h);
            Graphics grap = Graphics.FromImage((Image)btm);
            grap.DrawImage(img, 0, 0, w, h);
            grap.Dispose();
            //return the image
            pictureBox1.Image = (Image)btm;
            textBox1.Text = zoom.ToString();
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.Text = e.X.ToString() + "," + e.Y.ToString();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox3.Text = e.X.ToString() + "," + e.Y.ToString();
        }

        private void pictureBox1_MouseHover(object sender, EventArgs e)
        {
            pictureBox1.Focus();
        }

        private int a = 105;

        private void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0 && a == 200)
            {
                //textBox4.Text = a.ToString();
            }
            else
            {
                if (e.Delta > 0 && a >= 5 && a < 200)
                {
                    a += 5;
                    //textBox4.Text = a.ToString();
                }
            }
            if (e.Delta < 0 && a == 5)
            {
                //textBox4.Text = a.ToString();
            }
            else
            {
                if (e.Delta < 0 && a > 5 && a <= 200)
                {
                    a -= 5;
                    //textBox4.Text = a.ToString();
                }
            }

            Bitmap tamp = new Bitmap(img);
            int w, h;
            w = tamp.Width * a / 100;
            h = tamp.Height * a / 100;
            Bitmap btm = new Bitmap(w, h);
            Graphics grap = Graphics.FromImage((Image)btm);
            grap.DrawImage(img, 0, 0, w, h);
            grap.Dispose();
            //return the image
            pictureBox1.Image = (Image)btm;
            textBox1.Text = a.ToString();
        }
    }
}