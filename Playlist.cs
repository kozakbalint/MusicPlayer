using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicPlayer;

namespace MusicPlayer
{
    class Playlist
    {
        private List<Song> songs;
        private string name;

        public Playlist(string playlistName)
        {
            songs = new List<Song>();
            name = playlistName;
        }

        public void AddSong(Song song)
        {
            songs.Add(song);
        }

        public void RemoveSong(Song song)
        {
            foreach (var item in songs)
            {
                if (item.GetTitle == song.GetTitle)
                {
                    songs.Remove(item);
                }
            }
        }

        public void Save()
        {
            StreamWriter sw = new StreamWriter($"D:\\dev\\MusicPlayer\\Playlists\\{name}.pl");
            sw.WriteLine(name);
            foreach (var item in songs)
            {
                sw.WriteLine(item.GetPath);
            }
        }

        public void Load(string path)
        {
            StreamReader sr = new StreamReader(path);
            name = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                songs.Add(new Song(sr.ReadLine()));
            }
        }
    }
}
