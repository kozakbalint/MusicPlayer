using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TagLib;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Player musicPlayer;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 500);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var value = musicPlayer.GetPosition();
            slider.Value = value;
            asd.Content = $"{TimeSpan.FromSeconds(Math.Round(value,0))}";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            musicPlayer.Dispose();
            timer.Stop();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            musicPlayer.PlayPause(1.5);
            if (musicPlayer.GetPlaybackState == NAudio.Wave.PlaybackState.Playing)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        private void Stop(object sender, RoutedEventArgs e)
        {
            musicPlayer.Stop();
            timer.Stop();
        }
        private void Load(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if (btn.Name == "csocs")
            {
                musicPlayer = new Player("D:\\dev\\MusicPlayer\\Music\\BSW - Csöcsök és segg.mp3", 1);
                var tfile = TagLib.File.Create("D:\\dev\\MusicPlayer\\Music\\BSW - Csöcsök és segg.mp3");
                title.Content = $"{tfile.Tag.Title}";
            }
            else if (btn.Name == "toki")
            {
                musicPlayer = new Player("D:\\dev\\MusicPlayer\\Music\\Migos_-_Walk_It_Talk_It_ft._Drake_(Audio).mp3", 1);
                var tfile = TagLib.File.Create("D:\\dev\\MusicPlayer\\Music\\Migos_-_Walk_It_Talk_It_ft._Drake_(Audio).mp3");
                title.Content = $"{tfile.Tag.Title}";
            }
            else
            {
                musicPlayer = new Player("D:\\dev\\MusicPlayer\\Music\\ANTILOPE_KID_-_SZIA_URAM.mp3", 1);
                var tfile = TagLib.File.Create("D:\\dev\\MusicPlayer\\Music\\ANTILOPE_KID_-_SZIA_URAM.mp3");
                title.Content = $"{tfile.Tag.Title}";
            }
            slider.Maximum = musicPlayer.GetLenght();
            musicPlayer.Play(NAudio.Wave.PlaybackState.Stopped, 1);
            timer.Start();
            asd2.Content = $"{TimeSpan.FromSeconds(Math.Round(musicPlayer.GetLenght(),0))}";
        }

        private void SetValue(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            slider.IsMoveToPointEnabled = true;
            if (slider.IsMouseOver)
            {
                musicPlayer.SetPosition(slider.Value);
            }
        }

        private void SteVolume(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            volume.IsMoveToPointEnabled = true;
            musicPlayer.SetVolume((float)volume.Value);
        }
    }
}
