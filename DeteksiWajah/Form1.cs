using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV.CvEnum;


namespace DeteksiWajah
{
    public partial class Form1 : Form
    {
        Capture capture;
        bool captureprogress;
        HaarCascade haar;
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_Click(object sender, EventArgs e)
        {
            if (capture == null)
          {
                try
            {
                capture = new Capture();
            }
            catch (NullReferenceException exc)
            {
                MessageBox.Show("camera tidak aktif");
            }
          }

            if (capture != null)
            {
                if (captureprogress)
                {
                    Application.Idle -= prosesframe;
                }
                else
                {
                    Application.Idle += prosesframe;
                }
                captureprogress = !captureprogress;
            }
            
        }

        private void prosesframe(object sender, EventArgs e)
        {
            Image<Bgr, Byte> imageFrame = capture.QueryFrame();
            if(imageFrame!= null) 
            {
                Image<Gray,Byte> grayframe = imageFrame.Convert<Gray, Byte>();
                var face = grayframe.DetectHaarCascade(haar, 1.4, 4, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20))[0];
                foreach (var faces in face)
                {
                    imageFrame.Draw(faces.rect, new Bgr(Color.Red), 3);
                }
            }
            imageBox1.Image = imageFrame;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            haar = new HaarCascade("haarcascade_frontalface_default.xml");
        }
    }
}