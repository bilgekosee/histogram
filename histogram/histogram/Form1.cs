using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace histogram
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public double X_kaydirma { get; private set; }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = " Resimler|*.bmp|All files|*.*";
            if(sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }
            pictureBox1.ImageLocation = sfd.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ArrayList DiziPiksel = new ArrayList();

            int OrtalamaRenk = 0;
            Color OkunanRenk;
            int R = 0, G = 0, B = 0;
            Bitmap GirisResmi; //histogram için giriş gri ton olmalıdır
            GirisResmi = new Bitmap(pictureBox1.Image);

            int ResimGenisligi = GirisResmi.Width;
            int ResimYuksekligi = GirisResmi.Height;
            int i = 0; //piksel sayısı tutulacak
            for(int x=0; x<ResimGenisligi;x++)
            {
                for(int y=0; y< ResimYuksekligi; y++)
                {
                    OkunanRenk = GirisResmi.GetPixel(x, y);
                    OrtalamaRenk = (int)(OkunanRenk.R + OkunanRenk.G + OkunanRenk.B) / 3;//griton resimde üç kanal aynı değere sahiptir.

                    DiziPiksel.Add(OrtalamaRenk);//resimdeki tüm noktaları diziye atıyor

                }
            }
            int[] DiziPikselSayilari = new int[256];
            for(int r=0; r<=255;r++)//256 tane renk tonu için dönecek
            {
                int PikselSayisi = 0;
                for(int s=0; s<= DiziPiksel.Count;s++)//resimdeki piksel sayısınca dönecek
                {
                    if (r == Convert.ToInt16(DiziPiksel[s]))
                    {
                        PikselSayisi++;
                    }
                }
                DiziPikselSayilari[r] = PikselSayisi;

            }
            //değerleri listboxa ekliyor
            int RenkMaksPikselSayisi = 0;//grafikte y eksenini ölçerken kullanılacak
            for(int k=0;k<=255;k++)
            {
                listBox1.Items.Add("renk:" + k + "=" + DiziPikselSayilari[k]);
                //maksimum piksel sayisini bulmaya çalışıyor
                if(DiziPikselSayilari[k]>RenkMaksPikselSayisi)
                {
                    RenkMaksPikselSayisi = DiziPikselSayilari[k];
                }
            }
            //grafiği çiziyor
            Graphics CizimAlani;
            Pen Kalem1 = new Pen(System.Drawing.Color.Yellow, 1);
            Pen Kalem2 = new Pen(System.Drawing.Color.Red, 1);
            CizimAlani = pictureBox2.CreateGraphics();

            pictureBox2.Refresh();
            int GrafikYuksekligi = 300;
            double OlcekY = RenkMaksPikselSayisi / GrafikYuksekligi;
            double OlcekX = 1.5;
            int x_kaydirma = 10;
                for(int x=0; x<255;x++)
            {
                if (x % 50 == 0)
                    CizimAlani.DrawLine(Kalem2, (int)(x_kaydirma + x * OlcekX), GrafikYuksekligi, (int)(x_kaydirma + x * OlcekX), 0);
                    CizimAlani.DrawLine(Kalem1, (int)(x_kaydirma + x * OlcekX), GrafikYuksekligi,(int)(x_kaydirma + x * OlcekX), (GrafikYuksekligi - (int)(DiziPikselSayilari[x] / OlcekY)));


            }
            textBox1.Text = "maks.piks=" + RenkMaksPikselSayisi.ToString();
        }
    }
}
