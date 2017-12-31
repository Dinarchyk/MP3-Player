using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MP3_Player
{
    public partial class MainForm : Form
    {
        private static bool FlagForMute = true;
        private static double Time;
        private static double TimeCoefficient = 1;
        private static int FlagForTrackScrollVolume = 50;
        private static double MediaDuration;
        private static double MediaChangedFlag;
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            trackBarVolume.Value = 50;
            trackBar1.Value = 100;
            axWindowsMediaPlayer1.settings.volume = 50;
            axWindowsMediaPlayer1.settings.balance = 0;
            axWindowsMediaPlayer1.settings.rate = 1;
            TimeCoefficient = 1;
        }

        private void buttonPlay_Click(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playlistCollection.ToString() != null || axWindowsMediaPlayer1.playlistCollection.ToString() != "")
            {
                MediaDuration = axWindowsMediaPlayer1.currentMedia.duration;
                if (axWindowsMediaPlayer1.playState != WMPLib.WMPPlayState.wmppsPlaying)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                    timer.Start();
                }
                else
                {
                    timer.Stop();
                    axWindowsMediaPlayer1.Ctlcontrols.pause();
                }
            }
            else
            {
                var messageBox = MessageBox.Show("Load files first", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            Time = 0;
            timer.Stop();
            axWindowsMediaPlayer1.Ctlcontrols.stop();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            Time = 0;
            axWindowsMediaPlayer1.Ctlcontrols.next();
        }

        private void buttonPrev_Click(object sender, EventArgs e)
        {
            Time = 0;
            axWindowsMediaPlayer1.Ctlcontrols.previous();
        }

        private void buttonMute_Click(object sender, EventArgs e)
        {
            if (FlagForMute)
            {
                axWindowsMediaPlayer1.settings.mute = true;
                FlagForTrackScrollVolume = trackBarVolume.Value;
                trackBarVolume.Value = 100;
                FlagForMute = false;
            }
            else
            {
                axWindowsMediaPlayer1.settings.mute = false;
                trackBarVolume.Value = FlagForTrackScrollVolume;
                FlagForMute = true;
            }
        }

        private void trackBarVolume_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = 100 - trackBarVolume.Value;
        }

        private void buttonFaster_Click(object sender, EventArgs e)
        {

            axWindowsMediaPlayer1.settings.rate = (2 * axWindowsMediaPlayer1.settings.rate) > 4 ?
            axWindowsMediaPlayer1.settings.rate : (2 * axWindowsMediaPlayer1.settings.rate);
            TimeCoefficient = axWindowsMediaPlayer1.settings.rate;
        }

        private void MainForm_SizeChanged_1(object sender, EventArgs e)
        {
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            OnSizeChanged(EventArgs.Empty);
        }

        private void axWindowsMediaPlayer1_MediaChange(object sender, AxWMPLib._WMPOCXEvents_MediaChangeEvent e)
        {
            //time = 0;
            timer.Start();
            labelName.Text = axWindowsMediaPlayer1.currentMedia.name;
            MediaDuration = axWindowsMediaPlayer1.currentMedia.duration;
            TimeCoefficient = 1;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.balance = -100 + trackBar1.Value;
        }

        private void buttonSlower_Click(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.rate = (double)Math.Max((0.5 * axWindowsMediaPlayer1.settings.rate), 0.1);
            TimeCoefficient = axWindowsMediaPlayer1.settings.rate;
        }

        private void buttonLoadFiles_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "mp3 Audio File (*.mp3)|*.mp3|Windows Media Audio File (*.wma)|*.wma|Waveform Audio File (*.wav)|*.wav|All Files(*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = false;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            var fileNameAndPath = openFileDialog1.FileNames;
                            var fileName = openFileDialog1.SafeFileNames;

                            for (int i = 0; i < openFileDialog1.SafeFileNames.Count(); i++)
                            {
                                var safedListviewItem = new string[] { fileName[i], fileNameAndPath[i] };
                                var listItem = new ListViewItem(safedListviewItem);
                                listView1.Items.Add(listItem);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original" + ex.Source);
                }
            }
            label3.Visible = true;
        }

        private void buttonPlayAll_Click(object sender, EventArgs e)
        {
            //timer.Stop();
            if (listView1.Items.Count > 0)
            {
                Time = 0;
                var playList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myPlayList");
                WMPLib.IWMPMedia media;
                for (int i = 0; i < listView1.Items.Count; i++)
                {
                    int j = 1;
                    media = axWindowsMediaPlayer1.newMedia(listView1.Items[i].SubItems[j].Text);
                    playList.appendItem(media);
                    j++;
                    axWindowsMediaPlayer1.currentPlaylist = playList;
                }
                MediaDuration = axWindowsMediaPlayer1.currentMedia.duration;
                MediaChangedFlag = MediaDuration;
                axWindowsMediaPlayer1.Ctlcontrols.play();
                //timer.Start();
            }
            else
            {
                var messageBox = MessageBox.Show("Load files first", "", MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var graphics = e.Graphics;
            graphics.FillRectangle(Brushes.Lime, 80, 48, 213, 24);
            graphics.DrawRectangle(Pens.Plum, 80, 48, 213, 24);
            graphics.DrawRectangle(Pens.Plum, 79, 47, 213, 24);
            graphics.FillRectangle(Brushes.AliceBlue, 84, 54, (float)(MediaDuration == 0 ? 0 : 205 / MediaDuration * Time), 10);
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                timer.Stop();
                TimeCoefficient = 1;
            }
            Time = (Time > MediaDuration || (MediaDuration != MediaChangedFlag)) ? 0 : Time + TimeCoefficient;
            Invalidate();
            if (MediaDuration != MediaChangedFlag)
                MediaChangedFlag = MediaDuration;
            labelTime.Text = ((int)Time / 60).ToString() + ":" +
                (((Time % 60) > 9) ? ((int)(Time % 60)).ToString() : "0" + ((int)(Time % 60)).ToString()) + "/" +
                axWindowsMediaPlayer1.currentMedia.durationString;
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            var selectedFile = listView1.FocusedItem.SubItems[1].Text;
            axWindowsMediaPlayer1.URL = selectedFile;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            contextMenuStrip1.Show();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            var openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "mp3 Audio File (*.mp3)|*.mp3|Windows Media Audio File (*.wma)|*.wma|Waveform Audio File (*.wav)|*.wav|All Files(*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = false;
            openFileDialog1.Multiselect = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            var fileNameAndPath = openFileDialog1.FileNames;
                            var fileName = openFileDialog1.SafeFileNames;

                            for (int i = 0; i < openFileDialog1.SafeFileNames.Count(); i++)
                            {
                                var safedListviewItem = new string[] { fileName[i], fileNameAndPath[i] };
                                var listItem = new ListViewItem(safedListviewItem);
                                listView1.Items.Add(listItem);
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original" + ex.Source);
                }
            }
            label3.Visible = true;
        }

        private void playAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Time = 0;
            var playList = axWindowsMediaPlayer1.playlistCollection.newPlaylist("myPlayList");
            WMPLib.IWMPMedia media;
            for (int i = 0; i < listView1.Items.Count; i++)
            {
                int j = 1;
                media = axWindowsMediaPlayer1.newMedia(listView1.Items[i].SubItems[j].Text);
                playList.appendItem(media);
                j++;
                axWindowsMediaPlayer1.currentPlaylist = playList;
            }
            MediaDuration = axWindowsMediaPlayer1.currentMedia.duration;
            MediaChangedFlag = MediaDuration;
            axWindowsMediaPlayer1.Ctlcontrols.play();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog();
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new HelpForm();
            form.ShowDialog();
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (listView1.Items.Count > 0)
                listView1.FocusedItem.Remove();
            else
            {
                var messageBox = MessageBox.Show("Load files first", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
