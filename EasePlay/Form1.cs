using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasePlay
{
    public partial class Form1 : Form
    {
        private LibVLC _libVLC;
        private MediaPlayer _mediaPlayer;

        public Form1()
        {
            InitializeComponent();
            Core.Initialize();
            // 初始化 LibVLC 核心

            _libVLC = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVLC);

            _mediaPlayer.Hwnd = panel1.Handle;
            // 设置视频输出到 panel

            timer1.Interval = 1000;
            // 每秒更新一次进度
            timer1.Tick += Timer1_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "视频文件|*.mp4;*.avi;*.mkv|所有文件|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var media = new Media(_libVLC, new Uri(openFileDialog.FileName));
                    _mediaPlayer.Media = media;

                    if (_mediaPlayer.Media != null)
                    {
                        _mediaPlayer.Play();
                        timer1.Start();
                    }
                }
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (_mediaPlayer.Media != null)
            {
                _mediaPlayer.Play();
                timer1.Start();
            }
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            if (_mediaPlayer.IsPlaying)
            {
                _mediaPlayer.Pause();
                timer1.Stop();
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (_mediaPlayer.Length > 0)
            {
                trackBarProgress.Value = (int)(_mediaPlayer.Time * 100 / _mediaPlayer.Length);
                // 更新进度条
            }
        }

        private void trackBarProgress_Scroll(object sender, EventArgs e)
        {
            if (_mediaPlayer.Length > 0)
            {
                _mediaPlayer.Time = (long)(_mediaPlayer.Length * trackBarProgress.Value / 100);
                // 当 _mediaPlayer.Time 被赋值为某个时间点时，视频将跳转到该时间点开始播放
                // 我第一次看到这段代码的时候还怀疑是写反了，为什么是给 _mediaPlayer 赋值？进一步了解之后就理解了
            }
        }

    }
}
