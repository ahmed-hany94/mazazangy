using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace project_mazagangy
{
    public partial class mainForm : Form
    {
        string[] files, path;
        private AudioFileReader reader;
        private bool playing = false;
        private WaveEqualizerViewModel view;
        private WaveEqualizer EQ;
        private IWavePlayer player;
        private Action<float> setVolumeDelegate;
        private  PlaybackState playbackState;


        public mainForm()
        {
            Thread t = new Thread(new ThreadStart(screenStart));
            t.Start();
            Thread.Sleep(5000);
            InitializeComponent();
            t.Abort();
        }

        private ISampleProvider CreateInputStream(string fileName)
        {
            reader = new AudioFileReader(fileName);
            var sampleChannel = new SampleChannel(reader, true);
            sampleChannel.PreVolumeMeter += OnPreVolumeMeter;
            setVolumeDelegate = vol => sampleChannel.Volume = vol;
            var postVolumeMeter = new MeteringSampleProvider(sampleChannel);

            return postVolumeMeter;
        }
        void OnPreVolumeMeter(object sender, StreamVolumeEventArgs e)
        {

            waveformPainter1.AddMax(e.MaxSampleValues[0]);
            waveformPainter2.AddMax(e.MaxSampleValues[1]);
        }

       


        public void screenStart()
        {
            Application.Run(new Form2());

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Wait for the next update !!!", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

       

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            listBox1.SelectedItems.Clear();
            for(int i = listBox1.Items.Count -1; i>=0; i--)
            {
                if (listBox1.Items[i].ToString().ToLower().Contains(music_search.Text.ToLower()))
                {
                    listBox1.SetSelected(i, true);
                }


            }
            status.Text = listBox1.SelectedItems.Count.ToString() + " Item found ";
        }

        public void PlayPlaylist()
        {
            if (listBox1.Items.Count < 1) return;

            if (player != null && player.PlaybackState != PlaybackState.Stopped) player.Stop();

            if (reader != null) reader.Dispose();

            if (player != null) { player.Dispose(); player = null; }

            view = new WaveEqualizerViewModel();
            int index = listBox1.SelectedIndex + 1;
            ISampleProvider sampleProvider = CreateInputStream(path[index]);
            EQ = new WaveEqualizer(sampleProvider, view.GetBands);
            player = new WaveOutEvent();
            player.Init(EQ);
            player.Play();
            playbackState = PlaybackState.Playing;
        }


        private void open_music_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            WaveParser parser = new WaveParser();
            open.Multiselect = true;
            open.Filter = "Mp3 Files |* .mp3|Wav Files|*.wav";
            if (open.ShowDialog() == DialogResult.OK)
            {
                files = open.SafeFileNames;
                path = open.FileNames;
                parser.WaveHeaderIN(path[0]);
                view = new WaveEqualizerViewModel();
                int index = listBox1.SelectedIndex + 1;
                ISampleProvider sampleProvider = CreateInputStream(path[index]);
                EQ = new WaveEqualizer(sampleProvider, view.GetBands);
                for (int i = 0; i < files.Length; i++)
                {
                    listBox1.Items.Add(files[i]);
                }
                player = new WaveOutEvent();
                player.Init(EQ);
                player.Play();
                playbackState = PlaybackState.Playing;
                player.PlaybackStopped += new EventHandler<StoppedEventArgs>(player_PlaybackStopped);

            }
        }

        private void player_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            this.PlayPlaylist();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reader == null) return;
            else this.PlayPlaylist();
        }

        private void shuffleBtn_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            ListBox.ObjectCollection list = listBox1.Items;
            int x = list.Count;
            listBox1.BeginUpdate();
            while( x > 1)
            {
                x--;
                int y = r.Next(x + 1);
                object obj = list[y];
                list[y] = list[x];
                list[x] = obj;

            }
            listBox1.EndUpdate();
            listBox1.Invalidate();
        }

        private void remove_btn_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void clear_btn_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private void previous_btn_Click(object sender, EventArgs e)
        {

            int index = listBox1.SelectedIndex -1;
            if (index < 0)
                return;
            listBox1.SetSelected(index, true);

        }

        private void nxt_btn_Click(object sender, EventArgs e)
        {

            int index = listBox1.SelectedIndex +1;
            if (index >= listBox1.Items.Count )
                return;
            listBox1.SetSelected(index, true);
        }

        private void up_btn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item first !", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int newIndex = listBox1.SelectedIndex - 1;

                if (newIndex < 0)
                    return;
                object selectedItem = listBox1.SelectedItem;
                listBox1.Items.Remove(selectedItem);
                listBox1.Items.Insert(newIndex, selectedItem);
                listBox1.SetSelected(newIndex , true);
            
            }
        }

        private void down_btn_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an item first !", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                int newIndex = listBox1.SelectedIndex +1;

                if (newIndex >= listBox1.Items.Count)
                    return;
                object selectedItem = listBox1.SelectedItem;
                listBox1.Items.Remove(selectedItem);
                listBox1.Items.Insert(newIndex, selectedItem);
                listBox1.SetSelected(newIndex, true);
            }
        }

        private void Band1TrackBar_Scroll(object sender, EventArgs e)
        {

            if (reader != null)
            {
                view.Band1 = Band1TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band2TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band2 = Band2TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band3TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band3 = Band3TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band4TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band4 = Band4TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band5TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band5 = Band5TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band6TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band6 = Band6TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band7TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band7 = Band7TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band8TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band8 = Band8TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band9TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band9 = Band9TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band10TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band10 = Band10TrackBar.Value;
                EQ.Update();
            }
        }

        private void Band11TrackBar_Scroll(object sender, EventArgs e)
        {
            if (reader != null)
            {
                view.Band11 = Band11TrackBar.Value;
                EQ.Update();
            }
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            if (player == null) return;

            if (playbackState == PlaybackState.Paused)
            {
                PlayBtn.Text = "Play";
                player.Play();
                playbackState = PlaybackState.Playing;
            }
            else
            {
                PlayBtn.Text = "Pause";
                player.Pause();
                playbackState = PlaybackState.Paused;
            }

        }

        private void VolumeBar_Scroll(object sender, EventArgs e)
        {
            if (player != null)
            {
                VolumeBar.Maximum = 100;
                VolumeBar.Minimum = 0;
                reader.Volume = VolumeBar.Value;
            }
        }

        private void SeekBar_Scroll(object sender, EventArgs e)
        {
            reader.CurrentTime = TimeSpan.FromSeconds(SeekBar.Value);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(player != null)
            {
                SeekBar.Maximum = (int)reader.TotalTime.TotalSeconds;
                SeekBar.Value = (int)reader.CurrentTime.TotalSeconds;
            }
            
        }

    }
}
