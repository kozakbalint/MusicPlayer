using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MusicPlayer
{
    public partial class MainWindow : Window
    {
        Player mp;
        DispatcherTimer timer;
        Playlist pl;
        int currentIndex;
        double currentPos;
        float currentVolume = 1;
        bool repeate;
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            pl = new Playlist();
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0,0,0,0,500);
            timer.Start();
            title.Content = "None";
            artists.Content = "None";
            seekBar.Value = 0;
            seekBar.IsMoveToPointEnabled = true;
            volume.Value = currentVolume;
            volume.Maximum = 5;
            volume.IsMoveToPointEnabled = true;
        }
        private void Update(Song currentSong)
        {
            length.Content = TimeSpan.FromSeconds((int)mp.GetLenght()).ToString("mm\\:ss");
            title.Content = currentSong.GetTitle;
            if (currentSong.GetArtists.Length != 0)
            {
                artists.Content = currentSong.GetArtists[0];
            }
            else
            {
                artists.Content = "unknown";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (mp != null)
            {
                currentPos = mp.GetPosition();
                seekBar.Maximum = mp.GetLenght();
                seekBar.Value = currentPos;
                currentTime.Content = TimeSpan.FromSeconds((int)currentPos).ToString("mm\\:ss");
            }
        }

        private void PlayPause(object sender, RoutedEventArgs e)
        {
            if (mp != null)
            {
                mp.PlayPause(currentVolume);
            }
        }

        private void SelectSong(object sender, System.Windows.Input.MouseEventArgs e)
        {
            currentIndex = songs.SelectedIndex;
            if (mp == null)
            {
                mp = new Player(pl.songs[currentIndex].GetPath, 1);
                mp.Play(NAudio.Wave.PlaybackState.Stopped, currentVolume);
                Update(pl.songs[currentIndex]);
            }
            else
            {
                mp.Dispose();
                mp = null;
                SelectSong(sender, e);
            }
        }

        private void Prev(object sender, RoutedEventArgs e)
        {
            if (mp != null)
            {
                if (mp.GetPosition() > 3)
                {
                    mp.SetPosition(0);
                }
                else
                {
                    mp.Dispose();
                    mp = null;
                    if (currentIndex - 1 >= 0)
                    {
                        currentIndex--;
                        mp = new Player(pl.songs[currentIndex].GetPath, currentVolume);
                        mp.Play(NAudio.Wave.PlaybackState.Stopped, currentVolume);
                        Update(pl.songs[currentIndex]);                    
                    }
                    else
                    {
                        currentIndex = pl.songs.Count-1;
                        mp = new Player(pl.songs[currentIndex].GetPath, currentVolume);
                        mp.Play(NAudio.Wave.PlaybackState.Stopped, currentVolume);
                        Update(pl.songs[currentIndex]);                    
                    }
                }
            }
        }

        private void Repeat(object sender, RoutedEventArgs e)
        {
            if (mp != null)
            {
                if (repeate)
                {
                    mp.SetRepeate = false;
                    repeate = false;
                    repeatBtn.Content = "Repeat Off";
                }
                else
                {
                    mp.SetRepeate = true;
                    repeate = true;
                    repeatBtn.Content = "Repeat On";
                }
            }
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            if (mp != null)
            {
                mp.Dispose();
                mp = null;
                if (currentIndex+1 < pl.songs.Count)
                {
                    currentIndex++;
                    mp = new Player(pl.songs[currentIndex].GetPath, currentVolume);
                    mp.Play(NAudio.Wave.PlaybackState.Stopped, currentVolume);
                    Update(pl.songs[currentIndex]);
                }
                else
                {
                    currentIndex = 0;
                    mp = new Player(pl.songs[currentIndex].GetPath, currentVolume);
                    mp.Play(NAudio.Wave.PlaybackState.Stopped, currentVolume);
                    Update(pl.songs[currentIndex]);
                }
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            if (mp != null)
            {
                mp.Stop();
                title.Content = "None";
                artists.Content = "None";
                seekBar.Value = 0;
            }
        }

        private void OpenSong(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Audio files (*.wav, *.mp3, *.wma, *.ogg, *.flac) | *.wav; *.mp3; *.wma; *.ogg; *.flac";
            var result = ofd.ShowDialog();
            if (ofd.FileName != null)
            {
                pl.AddSong(new Song(ofd.FileName));
                UpdateList();
            }
        }

        private void UpdateList()
        {
            songs.Items.Clear();
            foreach (var item in pl.songs)
            {
                if (item.GetTitle == "" || item.GetTitle == null)
                {
                    songs.Items.Add("unknown");
                }
                else
                {
                    songs.Items.Add(item.GetTitle);
                } 
            }
        }

        private void SetSeekbar(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mp != null)
            {
                if (seekBar.IsMouseOver)
                {
                    mp.SetPosition(seekBar.Value);
                }
            }
        }

        private void SetVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (mp != null)
            {
                currentVolume = (float)volume.Value;
                mp.SetVolume(currentVolume);
            }
        }

        private void RemoveSong(object sender, RoutedEventArgs e)
        {
            var selected = songs.SelectedIndex;
            if (selected == currentIndex)
            {
                currentIndex = 0;
            }
            pl.RemoveSong(pl.songs[selected]);
            songs.Items.Remove(selected);
            UpdateList();
        }

        private void SavePlaylist(object sender, RoutedEventArgs e)
        {
            if (pl.songs.Count !=0)
            {
                var sfd = new SaveFileDialog();
                sfd.CreatePrompt = false;
                sfd.OverwritePrompt = true;
                sfd.Filter = "PLAYLIST files (*.pl) | *.pl";
                sfd.ShowDialog();
                pl.Save(System.IO.Path.GetFileNameWithoutExtension(sfd.FileName),sfd.FileName);
            }
        }

        private void LoadPlaylist(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "PLAYLIST files (*.pl) | *.pl";
            var result = ofd.ShowDialog();
            if (ofd.FileName != null)
            {
                pl.Load(ofd.FileName);
                UpdateList();
            }
        }
    }
}
