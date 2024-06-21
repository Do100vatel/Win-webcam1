using System;
using System.Drawing;
using System.Windows.Forms;
using DirectShowLib;

namespace WebcamApp
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public WebcamForm()
        {
            InitializeComponent();

            LoadCameras();
        }

        private void LoadCameras()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in videoDevices)
            {
                cbCameras.Items.Add(device.Name);
            }
            if (cbCameras.Items.Count > 0)
                cbCameras.SelectedIndex = 0;
        }

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (videoSource == null)
            {
                StartCamera();
                btnStartStop.Text = "Stop";
            }
            else
            {
                StopCamera();
                btnStartStop.Text = "Start";
            }
        }

        private void StartCamera()
        {
            if (cbCameras.SelectedIndex >= 0)
            {
                videoSource = new VideoCaptureDevice(videoDevices[cbCameras.SelectedIndex].MonikerString);
                videoSource.NewFrame += VideoSource_NewFrame;
                videoSource.Start();
            }
        }

        private void StopCamera()
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
                videoSource = null;
            }
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bitmap;
        }
    }
}
