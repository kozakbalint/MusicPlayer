using System;
using System.Collections.Generic;
using System.IO;

namespace MusicPlayer
{
    class Playlist
    {
        public List<Song> songs;
        private string name;

        public Playlist()
        {
            songs = new List<Song>();
        }

        public void AddSong(Song song)
        {
            songs.Add(song);
        }

        public void RemoveSong(Song song)
        {
            songs.Remove(song);
        }

        public void Save(string playlistName, string path)
        {
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine(playlistName);
            foreach (var item in songs)
            {
                sw.WriteLine(item.GetPath);
            }
            sw.Close();
        }

        public void Load(string path)
        {
            StreamReader sr = new StreamReader(path);
            name = sr.ReadLine();
            while (!sr.EndOfStream)
            {
                songs.Add(new Song(sr.ReadLine()));
            }
            sr.Close();
        }
    }
}
