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
    public partial class ListOfMisc : Form
    {
        public ListOfMisc()
        {
            InitializeComponent();
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

            if (openFileDialog1.ShowDialog()== DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        using (myStream)
                        {
                            var fileNameAndPath = openFileDialog1.FileNames;
                            var fileName = openFileDialog1.SafeFileNames;

                            for (int i= 0; i< openFileDialog1.SafeFileNames.Count();i++)
                            {
                                var safedListviewItem = new string[] {fileName[i], fileNameAndPath[i] };
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
        }

        private void buttonPlayAll_Click(object sender, EventArgs e)
        {
            var form1 = new MainForm();
            var playList = form1.axWindowsMediaPlayer1.playlistCollection.newPlaylist("myPlayList");
            WMPLib.IWMPMedia media;
            for(int i=0; i< listView1.Items.Count;i++)
            {
                int j = 1;
                media = form1.axWindowsMediaPlayer1.newMedia(listView1.Items[i].SubItems[j].Text);
                playList.appendItem(media);
                j++;
                form1.axWindowsMediaPlayer1.currentPlaylist = playList;
                form1.axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            form1.ShowDialog();
        }

        private void ListOfMisc_Resize(object sender, EventArgs e)
        {

        }
    }
}
