using MessagingToolkit.QRCode.Codec.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qr_test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Create a new bitmap.
            var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                           Screen.PrimaryScreen.Bounds.Height);

            // Create a graphics object from the bitmap.
            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                        Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);
            pictureBox1.Image = bmpScreenshot;
        }

        private void qr_encode()
        {
            using (SaveFileDialog sfd = new SaveFileDialog() { Filter = "JPEG|*jpg", ValidateNames = true, FileName = "qr_code.jpg" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    MessagingToolkit.QRCode.Codec.QRCodeEncoder encoder = new MessagingToolkit.QRCode.Codec.QRCodeEncoder();
                    encoder.QRCodeScale = 8;
                    Bitmap bmp = encoder.Encode(textBox1.Text);
                    pictureBox2.Image = bmp;
                    bmp.Save(sfd.FileName, ImageFormat.Jpeg);

                }
            }
        }
        public void qr_decode()
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "JPEG|*jpg", ValidateNames = true, Multiselect = false })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox3.Image = Image.FromFile(ofd.FileName);
                    MessagingToolkit.QRCode.Codec.QRCodeDecoder decoder = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
                    textBox2.Text = decoder.Decode(new QRCodeBitmapImage(pictureBox3.Image as Bitmap));
                }
            }
        }

        private void qr_decode_X()
        {
        //--------------------------------------------------------------------------------------------------

            int x;
            int y;
            string I;
            string II;

            I = Properties.Settings.Default["x"].ToString();
            II = Properties.Settings.Default["y"].ToString();

            x = Convert.ToInt32(I);
            y = Convert.ToInt32(II);

        //--------------------------------------------------------------------------------------------------

            try
            {
                MessagingToolkit.QRCode.Codec.QRCodeDecoder decoder = new MessagingToolkit.QRCode.Codec.QRCodeDecoder();
                textBox2.Text = decoder.Decode(new QRCodeBitmapImage(pictureBox3.Image as Bitmap));
                MessageBox.Show("Func");
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Fehler");
                x++;
                y++;
                Properties.Settings.Default["x"] = x;
                Properties.Settings.Default["y"] = y;
                Properties.Settings.Default.Save();

                MessageBox.Show(x.ToString()+" :qr_decoder_X");
                SearchPx();
                return;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            qr_encode();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            qr_decode();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd_2 = new OpenFileDialog() { Filter = "JPEG|*jpg|PNG|*png", ValidateNames = true, Multiselect = false })
            {
                if (ofd_2.ShowDialog() == DialogResult.OK)
                {
                    Bitmap bitmap_1 = new Bitmap(ofd_2.FileName);
                    pictureBox4.Image = bitmap_1;
                    Properties.Settings.Default["import"] = ofd_2.FileName;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Properties.Settings.Default["import"].ToString());
            Form2 frm2 = new Form2();
            frm2.Show();
            SearchPx();
            frm2.Close();
        }

        private void SearchPx()
        {
        //--------------------------------------------------------------------------------------------------

            int x;
            int y;
            string I;
            string II;
            int length;
            //int max;
            //int min;

            I = Properties.Settings.Default["x"].ToString();
            II = Properties.Settings.Default["y"].ToString();

            x = Convert.ToInt32(I);
            y = Convert.ToInt32(II);

            //MessageBox.Show(x+"x"+y);

        //--------------------------------------------------------------------------------------------------

            Bitmap bitmap = new Bitmap(1, 1);
            bitmap = (Bitmap)Image.FromFile(Properties.Settings.Default["import"].ToString());

        //--------------------------------------------------------------------------------------------------

            if (bitmap.Height <= 177)
            {
                MessageBox.Show("Kleiner als 177x177");
                return;
            }

            //MessageBox.Show("Größer als 177x177");
            length = 177;

            for (y = 0; y < bitmap.Height; y++)
            {
                for (x = 0; x < bitmap.Width; x++)
                {
                    if (bitmap.GetPixel(x, y).GetBrightness() <= 0.1)
                    {
                        Properties.Settings.Default["x"] = x;
                        Properties.Settings.Default["y"] = y;
                        Properties.Settings.Default["length"] = length;
                        Properties.Settings.Default.Save();

                        SearchCode();
                        return;
                    }
                }
            }
        }

        private void SearchCode()
        {
        //--------------------------------------------------------------------------------------------------

            int x;
            int y;
            int length;
            string I;
            string II;
            string III;
            int max;
            int min;

            I = Properties.Settings.Default["x"].ToString();
            II = Properties.Settings.Default["y"].ToString();
            III = Properties.Settings.Default["length"].ToString();

            x = Convert.ToInt32(I);
            y = Convert.ToInt32(II);
            length = Convert.ToInt32(III);

            MessageBox.Show(x.ToString() + " :SearchCode");
            MessageBox.Show(Properties.Settings.Default["x"].ToString() + " :SearchCode");

            //--------------------------------------------------------------------------------------------------

            Bitmap bitmap = new Bitmap(1, 1);
            bitmap = (Bitmap)Image.FromFile(Properties.Settings.Default["import"].ToString());

            Bitmap qr_code = new Bitmap(1, 1);

        //--------------------------------------------------------------------------------------------------

            //MessageBox.Show(x + "x" + y);

            min = x + 21;
            max = x + length;
            while (max > min)
            {
                if (bitmap.GetPixel(max, y).GetBrightness() <= 0.1)
                {
                    length = max - x;
                    //MessageBox.Show(x.ToString()+": x");
                    //MessageBox.Show(max.ToString() + ": max");
                    //MessageBox.Show(length.ToString() + ": length");

                    //WEITER BAUEN!!!!!!!!!!!!!


                    qr_code = bitmap.Clone(new Rectangle(x, y, length, length), PixelFormat.DontCare);
                    pictureBox4.Image = qr_code;
                    pictureBox3.Image = qr_code;
                    qr_decode_X();
                    return;
                }
                max--;
            }
        }
    }   
}
